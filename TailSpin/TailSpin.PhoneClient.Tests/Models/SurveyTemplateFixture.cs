namespace TailSpin.PhoneClient.Tests.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PhoneClient.Models;

    [Tag("Model")]
    [TestClass]
    public class SurveyTemplateFixture
    {
        [TestMethod]
        public void WhenCreateTheNewSurveyIsNotComplete()
        {
            var template = new SurveyTemplate
            {
                Questions = new List<Question>
                                {
                                    new Question { Text = "SimpleText Question", Type = QuestionType.SimpleText },
                                }
            };
            var newSurvey = template.CreateSurveyAnswers();

            Assert.IsFalse(newSurvey.IsComplete);
        }

        [TestMethod]
        public void WhenCreateTheNewSurveyHasSameTitleAsTemplate()
        {
            var template = new SurveyTemplate { Questions = new List<Question>(), Title = "My Title" };
            var newSurvey = template.CreateSurveyAnswers();

            Assert.AreEqual(template.Title, newSurvey.Title);
        }

        [TestMethod]
        public void WhenCreateTheNewSurveyHasSameTenantAsTemplate()
        {
            var template = new SurveyTemplate { Questions = new List<Question>(), Tenant = "My Tenant" };
            var newSurvey = template.CreateSurveyAnswers();

            Assert.AreEqual(template.Tenant, newSurvey.Tenant);
        }

        [TestMethod]
        public void WhenCreateTheNewSurveyHasSameSlugNameAsTemplate()
        {
            var template = new SurveyTemplate { Questions = new List<Question>(), SlugName = "SlugName" };
            var newSurvey = template.CreateSurveyAnswers();

            Assert.AreEqual(template.SlugName, newSurvey.SlugName);
        }

        [TestMethod]
        public void WhenCreateTheNewSurveyHasAnswerTypesAccordingToQuestionsType()
        {
            var template = new SurveyTemplate
                               {
                                   Questions = new List<Question>
                                                   {
                                                       new Question { Text = "SimpleText Question", Type = QuestionType.SimpleText },
                                                       new Question { Text = "FiveStars Question", Type = QuestionType.FiveStars },
                                                       new Question
                                                           {
                                                               Text = "MultipleChoice Question",
                                                               Type = QuestionType.MultipleChoice,
                                                               PossibleAnswers = new List<string>()
                                                           },
                                                       new Question { Text = "Picture Question", Type = QuestionType.Picture },
                                                       new Question { Text = "Voice Question", Type = QuestionType.Voice },
                                                   },
                               };
            var newSurvey = template.CreateSurveyAnswers();

            CollectionAssert.AreEquivalent(template.Questions.Select(q => q.Text).ToArray(), newSurvey.Answers.Select(a => a.QuestionText).ToArray());
            Assert.AreEqual(QuestionType.SimpleText, newSurvey.Answers.Where(a => a.QuestionText == "SimpleText Question").Single().QuestionType);
            Assert.AreEqual(QuestionType.FiveStars, newSurvey.Answers.Where(a => a.QuestionText == "FiveStars Question").Single().QuestionType);
            Assert.AreEqual(QuestionType.MultipleChoice, newSurvey.Answers.Where(a => a.QuestionText == "MultipleChoice Question").Single().QuestionType);
            Assert.AreEqual(QuestionType.Picture, newSurvey.Answers.Where(a => a.QuestionText == "Picture Question").Single().QuestionType);
            Assert.AreEqual(QuestionType.Voice, newSurvey.Answers.Where(a => a.QuestionText == "Voice Question").Single().QuestionType);
        }

        [TestMethod]
        public void WhenCreateMultipleChoiceAnswersKeepPossibleAnswers()
        {
            var template = new SurveyTemplate
                               {
                                   Questions = new List<Question>
                                                   {
                                                       new Question
                                                           {
                                                               Text = "Question One",
                                                               Type = QuestionType.MultipleChoice,
                                                               PossibleAnswers = new List<string> { "a", "b", "c" }
                                                           },
                                                       new Question
                                                           {
                                                               Text = "Question Two",
                                                               Type = QuestionType.MultipleChoice,
                                                               PossibleAnswers = new List<string> { "1", "2", "3", "4" }
                                                           },
                                                   },
                               };
            var newSurvey = template.CreateSurveyAnswers();

            CollectionAssert.AreEquivalent(
                newSurvey.Answers.Where(a => a.QuestionText == "Question One").Single().PossibleAnswers.ToArray(),
                template.Questions[0].PossibleAnswers);
            CollectionAssert.AreEquivalent(
                newSurvey.Answers.Where(a => a.QuestionText == "Question Two").Single().PossibleAnswers.ToArray(),
                template.Questions[1].PossibleAnswers);
        }
    }
}