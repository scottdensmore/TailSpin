//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Tests.Models
{
    using System.Collections.Generic;
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.PhoneClient.Models;

    [Tag("Model")]
    [TestClass]
    public class SurveyAnswerFixture
    {
        [TestMethod]
        public void WhenCreatedItsNotCompleted()
        {
            var template = new SurveyTemplate
            {
                Questions = new List<Question>
                                            {
                                                new Question { Text = "Question1", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question2", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question3", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question4", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question5", Type = QuestionType.SimpleText },
                                            },
            };

            var answer = template.CreateSurveyAnswers();

            Assert.IsFalse(answer.IsComplete);
        }

        [TestMethod]
        public void IfOnlySomeQuestionsHaveAnswersItsNotCompleted()
        {
            var template = new SurveyTemplate
            {
                Questions = new List<Question>
                                            {
                                                new Question { Text = "Question1", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question2", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question3", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question4", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question5", Type = QuestionType.SimpleText },
                                            },
            };

            var answer = template.CreateSurveyAnswers();
            answer.Answers[0].Value = "some answer";
            answer.Answers[1].Value = "some answer";

            Assert.IsFalse(answer.IsComplete);
        }

        [TestMethod]
        public void CompletenessIsCalculatedBasedOnCurrentAnswers()
        {
            var template = new SurveyTemplate
            {
                Questions = new List<Question>
                                            {
                                                new Question { Text = "Question1", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question2", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question3", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question4", Type = QuestionType.SimpleText },
                                                new Question { Text = "Question5", Type = QuestionType.SimpleText },
                                            },
            };

            var answer = template.CreateSurveyAnswers();
            var noanswersYet = answer.CompletenessPercentage;
            answer.Answers[0].Value = "answer1";
            var oneAnswer = answer.CompletenessPercentage;
            answer.Answers[1].Value = "answer2";
            answer.Answers[2].Value = "answer3";
            var threeAnswers = answer.CompletenessPercentage;
            answer.Answers[3].Value = "answer4";
            answer.Answers[4].Value = "answer5";
            var fiveAnswers = answer.CompletenessPercentage;

            Assert.IsFalse(answer.IsComplete);
            Assert.AreEqual(0, noanswersYet);
            Assert.AreEqual(20, oneAnswer);
            Assert.AreEqual(60, threeAnswers);
            Assert.AreEqual(100, fiveAnswers);
        }
    }
}
