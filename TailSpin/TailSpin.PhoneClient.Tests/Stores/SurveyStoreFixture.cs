namespace TailSpin.PhoneClient.Tests.Stores
{
    using System;
    using System.Collections.Generic;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PhoneClient.Models;
    using TailSpin.PhoneClient.Services.Stores;

    [Tag("Store")]
    [TestClass]
    public class SurveyStoreFixture
    {
        private static List<string> storeNames;

        [ClassInitialize]
        public static void InitializeStoreNames()
        {
            storeNames = new List<string>();
        }

        [ClassCleanup]
        public static void RemoveTemporaryStoreFiles()
        {
            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                storeNames.ForEach(
                    storeName =>
                        {
                            if (filesystem.FileExists(storeName))
                            {
                                filesystem.DeleteFile(storeName);
                            }
                        });
            }
        }

        [TestMethod]
        public void WhenCreatedStoreIsEmpty()
        {
            var store = new SurveyStore(GetNewStoreName());
            var templates = store.GetSurveyTemplates();
            Assert.AreEqual(0, templates.Count());
        }

        [TestMethod]
        public void WhenTemplateIsSavedItIsAccessibleFromStore()
        {
            var store = new SurveyStore(GetNewStoreName());
            var template = new SurveyTemplate { Tenant = "T1", SlugName = "S1" };
            store.SaveSurveyTemplates(new[] { template });
            var templates = store.GetSurveyTemplates();
            Assert.AreEqual(1, templates.Count());
            Assert.AreEqual(template, templates.Single());
        }

        [TestMethod]
        public void WhenTemplateIsSavedItIsAccessibleFromANewStoreWithSameName()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            var template = new SurveyTemplate { Tenant = "T1", SlugName = "S1" };
            store.SaveSurveyTemplates(new[] { template });
            var newStore = new SurveyStore(storeName);
            var templates = newStore.GetSurveyTemplates();
            Assert.AreEqual(1, templates.Count());
            Assert.IsTrue(TemplatesAreEqual(template, templates.Single()));
        }

        [TestMethod]
        public void WhenSimpleTextAnswerIsSavedItIsAccessibleFromANewStoreWithSameName()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            var template = new SurveyTemplate { Tenant = "T1", SlugName = "S1" };
            template.Questions.Add(new Question { Type = QuestionType.SimpleText, Text = "MyQuestion" });
            var answer = template.CreateSurveyAnswers();
            answer.IsComplete = true;
            answer.Answers[0].Value = "default";

            store.SaveSurveyTemplates(new[] { template });
            store.SaveSurveyAnswer(answer);

            var newStore = new SurveyStore(storeName);
            var templates = newStore.GetSurveyTemplates();
            var answers = newStore.GetCompleteSurveyAnswers();

            Assert.AreEqual(1, templates.Count());
            Assert.IsTrue(TemplatesAreEqual(template, templates.Single()));
            Assert.IsTrue(AnswersAreEqual(answer, answers.Single()));
        }

        [TestMethod]
        public void WhenQuestionAnswerIsSavedItIsAccessibleFromANewStoreWithSameName()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            var template = new SurveyTemplate { Tenant = "T1", SlugName = "S1" };
            var question = new Question { Type = QuestionType.MultipleChoice, Text = "MyQuestion" };
            question.PossibleAnswers.AddRange(new[] { "A", "B", "C" });
            template.Questions.Add(question);

            var answer = template.CreateSurveyAnswers();
            answer.Answers[0].Value = "value0";
            answer.IsComplete = true;
            store.SaveSurveyTemplates(new[] { template });
            store.SaveSurveyAnswer(answer);

            var newStore = new SurveyStore(storeName);
            var templates = newStore.GetSurveyTemplates();
            var answers = newStore.GetCompleteSurveyAnswers();

            Assert.AreEqual(1, templates.Count());
            Assert.IsTrue(TemplatesAreEqual(template, templates.Single()));
            Assert.IsTrue(AnswersAreEqual(answer, answers.Single()));
        }

        [TestMethod]
        public void WhenMultipleQuestionsAnswerIsSavedItIsAccessibleFromANewStoreWithSameName()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            var template = new SurveyTemplate { Tenant = "T1", SlugName = "S1" };
            var question = new Question { Type = QuestionType.MultipleChoice, Text = "MyQuestion" };
            question.PossibleAnswers.AddRange(new[] { "A", "B", "C" });
            template.Questions.Add(question);
            template.Questions.Add(new Question { Type = QuestionType.SimpleText, Text = "MyQuestion" });

            var answer = template.CreateSurveyAnswers();
            answer.IsComplete = true;
            answer.Answers[0].Value = "value0";
            answer.Answers[1].Value = "value1";
            store.SaveSurveyTemplates(new[] { template });
            store.SaveSurveyAnswer(answer);

            var newStore = new SurveyStore(storeName);
            var templates = newStore.GetSurveyTemplates();
            var answers = newStore.GetCompleteSurveyAnswers();

            Assert.AreEqual(1, templates.Count());
            Assert.IsTrue(TemplatesAreEqual(template, templates.Single()));
            Assert.IsTrue(AnswersAreEqual(answer, answers.Single()));
        }

        [TestMethod]
        public void WhenCreatedLastSyncDateIsEmpty()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);

            Assert.AreEqual(string.Empty, store.LastSyncDate);
        }

        [TestMethod]
        public void WhenLastSyncDateIsStoredItsAccessible()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            store.LastSyncDate = "foo";

            Assert.AreEqual("foo", store.LastSyncDate);
        }

        [TestMethod]
        public void WhenLastSyncDateIsStoredCanBeAccessedInAnotherStoreWithTheSameName()
        {
            var storeName = GetNewStoreName();
            var store = new SurveyStore(storeName);
            store.LastSyncDate = "foo";

            var storeForReading = new SurveyStore(storeName);

            Assert.AreEqual("foo", storeForReading.LastSyncDate);
        }

        private static bool TemplatesAreEqual(SurveyTemplate t1, SurveyTemplate t2)
        {
            if (t1.Tenant == t2.Tenant && t1.SlugName == t2.SlugName && t1.Questions.Count == t2.Questions.Count)
            {
                return t1.Questions.All(q1 =>
                    t2.Questions.Any(q2 =>
                        q1.Text == q2.Text
                        && q1.Type == q2.Type
                        && q1.PossibleAnswers.Intersect(q2.PossibleAnswers).Count() == q1.PossibleAnswers.Count
                        && q1.PossibleAnswers.Intersect(q2.PossibleAnswers).Count() == q2.PossibleAnswers.Count));
            }
            return false;
        }

        private static bool AnswersAreEqual(SurveyAnswer t1, SurveyAnswer t2)
        {
            if (t1.Tenant == t2.Tenant && t1.SlugName == t2.SlugName && t1.Answers.Count == t2.Answers.Count)
            {
                return t1.Answers.All(q1 =>
                    t2.Answers.Any(q2 =>
                        q1.QuestionText == q2.QuestionText
                        && q1.Value == q2.Value
                        && q1.GetType() == q2.GetType()
                        && q1.GetType() == typeof(QuestionAnswer) ?
                                        (q1.PossibleAnswers.Intersect(q2.PossibleAnswers).Count()
                                            == q1.PossibleAnswers.Count
                                         && q1.PossibleAnswers.Intersect(q2.PossibleAnswers).Count()
                                            == q2.PossibleAnswers.Count)
                                        : true));
            }
            return false;
        }

        private static string GetNewStoreName()
        {
            var storeName = Guid.NewGuid().ToString();
            storeNames.Add(storeName);
            return storeName;
        }
    }
}
