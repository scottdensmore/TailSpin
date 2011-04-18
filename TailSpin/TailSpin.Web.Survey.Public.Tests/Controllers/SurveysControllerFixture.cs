




 




namespace TailSpin.Web.Survey.Public.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TailSpin.Web.Survey.Public.Controllers;
    using TailSpin.Web.Survey.Public.Models;
    using TailSpin.Web.Survey.Shared.Models;
    using TailSpin.Web.Survey.Shared.Stores;

    [TestClass]
    public class SurveysControllerFixture
    {
        [TestMethod]
        public void DisplayCallsStoreWithTenantAndIdForSurveyToGet()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(new Survey());
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            controller.Display("tenant", "slug");

            mockSurveyStore.Verify(
                r => r.GetSurveyByTenantAndSlugName(
                    It.Is<string>(t => "tenant" == t),
                    It.Is<string>(s => "slug" == s),
                    It.Is<bool>(b => b)),
                    Times.Once());
        }

        [TestMethod]
        public void DisplayReturnsEmptyViewName()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(new Survey());
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void DisplayCopiesTheSurveyTitleToTheSurveyAnswerRetrnedInTheContentModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey { Title = "title to be copied" };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswer>;
            Assert.AreEqual("title to be copied", model.ContentModel.Title);
        }

        [TestMethod]
        public void DisplayCopiesTheSurveyQuestionTextToTheSurveyAnswerRetrnedInTheContentModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
                             {
                                 Questions = new List<Question>(
                                     new[]
                                     {
                                        new Question { Text = "question text to copy" },
                                     })
                             };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswer>;
            Assert.AreEqual("question text to copy", model.ContentModel.QuestionAnswers.First().QuestionText);
        }

        [TestMethod]
        public void DisplayCopiesTheSurveyQuestionTypeToTheQuestionAnswerReturnedInTheContentModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(
                    new[]
                                     {
                                        new Question { Type = QuestionType.SimpleText },
                                     })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswer>;
            Assert.AreEqual(QuestionType.SimpleText, model.ContentModel.QuestionAnswers.First().QuestionType);
        }

        [TestMethod]
        public void DisplayTransformsAllTheSurveyQuestionsToQuestionAnswerRetrnedInTheContentModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(
                    new[]
                                     {
                                        new Question(),
                                        new Question()
                                     })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswer>;
            Assert.AreEqual(2, model.ContentModel.QuestionAnswers.Count());
        }

        [TestMethod]
        public void DisplayReturnsSurveyTitleAsTitleInTheModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey { Title = "title" };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Display(string.Empty, string.Empty) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("title", model.Title);
        }

        [TestMethod]
        public void DisplayWhenPostReturnsRedirectToThankYouAction()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(new Survey());
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);

            var result = controller.Display(string.Empty, string.Empty, new SurveyAnswer()) as RedirectToRouteResult;

            Assert.AreEqual("ThankYou", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void DisplayWhenPostCallsStoreWithTenantAndIdForSurveyToGet()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(new Survey());
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);

            controller.Display("tenant", "slug", new SurveyAnswer());

            mockSurveyStore.Verify(
                r => r.GetSurveyByTenantAndSlugName(
                    It.Is<string>(t => "tenant" == t),
                    It.Is<string>(s => "slug" == s),
                    It.Is<bool>(b => b)),
                    Times.Once());
        }

        [TestMethod]
        public void DisplayWhenPostCallsSaveSurveyAnswerFromSurveyAnswerStoreWithTheTenantReadFromTheControllerWhenModelIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);

            controller.Display("tenant", string.Empty, new SurveyAnswer());

            mockSurveyAnswerStore.Verify(
                r => r.SaveSurveyAnswer(
                    It.Is<SurveyAnswer>(sa => "tenant" == sa.Tenant)),
                Times.Once());
        }

        [TestMethod]
        public void DisplayWhenPostCallsSaveSurveyAnswerFromSurveyAnswerStoreWithTheSlugParameterWhenModelIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey();
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);

            controller.Display(string.Empty, "slug", new SurveyAnswer());

            mockSurveyAnswerStore.Verify(
                r => r.SaveSurveyAnswer(
                    It.Is<SurveyAnswer>(sa => "slug" == sa.SlugName)),
                Times.Once());
        }

        [TestMethod]
        public void DisplayWhenPostCallsSaveSurveyAnswerFromSurveyAnswerStoreWithQuestionTextReadFromTheSurveyWhenModelIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(
                    new[]
                                     {
                                        new Question { Text = "question text" },
                                     })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer() })
            };

            controller.Display(string.Empty, string.Empty, surveyAnswer);

            mockSurveyAnswerStore.Verify(
                r => r.SaveSurveyAnswer(
                    It.Is<SurveyAnswer>(sa => "question text" == sa.QuestionAnswers[0].QuestionText)),
                Times.Once());
        }

        [TestMethod]
        public void DisplayWhenPostCallsSaveSurveyAnswerFromSurveyAnswerStoreWithQuestionTypeReadFromTheSurveyWhenModelIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(
                    new[]
                                     {
                                        new Question { Type = QuestionType.SimpleText },
                                     })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer() })
            };

            controller.Display(string.Empty, string.Empty, surveyAnswer);

            mockSurveyAnswerStore.Verify(
                r => r.SaveSurveyAnswer(
                    It.Is<SurveyAnswer>(sa => QuestionType.SimpleText == sa.QuestionAnswers[0].QuestionType)),
                Times.Once());
        }

        [TestMethod]
        public void DisplayWhenPostCallsSaveSurveyAnswerFromSurveyAnswerStoreWithAnswerReadFromTheParameterWhenModelIsValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(new[] { new Question() })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var mockSurveyAnswerStore = new Mock<ISurveyAnswerStore>();
            var controller = new SurveysController(mockSurveyStore.Object, mockSurveyAnswerStore.Object);
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer { Answer = "answer" } })
            };

            controller.Display(string.Empty, string.Empty, surveyAnswer);

            mockSurveyAnswerStore.Verify(
                r => r.SaveSurveyAnswer(
                    It.Is<SurveyAnswer>(sa => "answer" == sa.QuestionAnswers[0].Answer)),
                Times.Once());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DisplayWhenPostThrowsIfSaveSurveyAnswersReadFromSurveyStoreHaveDifferentCountToTheSurveyAnswerParameter()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var surveyWith1Question = new Survey
            {
                Questions = new List<Question>(new[] { new Question() })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(surveyWith1Question);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));
            var surveyAnswerWithoutQuestions = new SurveyAnswer();

            controller.Display(string.Empty, string.Empty, surveyAnswerWithoutQuestions);
        }

        [TestMethod]
        public void DisplayWhenPostReturnsModelWithTheAnswersReadFromTheParameterWhenModelIsNotValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(new[] { new Question() })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer { Answer = "answer" } })
            };

            var result = controller.Display(string.Empty, string.Empty, surveyAnswer) as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<SurveyAnswer>;
            Assert.AreEqual("answer", model.ContentModel.QuestionAnswers.First().Answer);
        }

        [TestMethod]
        public void DisplayWhenPostReturnsEmptyViewNameWhenModelIsNotValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Questions = new List<Question>(new[] { new Question() })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer() })
            };

            var result = controller.Display(string.Empty, string.Empty, surveyAnswer) as ViewResult;

            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void DisplayWhenPostReturnsSurveyTitleAsTitleInTheModelWhenModelIsNotValid()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var survey = new Survey
            {
                Title = "title",
                Questions = new List<Question>(new[] { new Question() })
            };
            mockSurveyStore.Setup(r => r.GetSurveyByTenantAndSlugName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                              .Returns(survey);
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));
            controller.ModelState.AddModelError("error for test", @"invalid model state");
            var surveyAnswer = new SurveyAnswer
            {
                QuestionAnswers = new List<QuestionAnswer>(new[] { new QuestionAnswer() })
            };

            var result = controller.Display(string.Empty, string.Empty, surveyAnswer) as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreSame("title", model.Title);
        }

        [TestMethod]
        public void IndexReturnsTitleInTheModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            var result = controller.Index() as ViewResult;

            var model = result.ViewData.Model as TenantMasterPageViewData;
            Assert.AreEqual("Existing surveys", model.Title);
        }

        [TestMethod]
        public void IndexGetsRecentSurveysFromStore()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));

            controller.Index();

            mockSurveyStore.Verify(r => r.GetRecentSurveys(), Times.Once());
        }

        [TestMethod]
        public void IndexReturnsRecentSurveysFromStoreInTheModel()
        {
            var mockSurveyStore = new Mock<ISurveyStore>();
            var controller = new SurveysController(mockSurveyStore.Object, default(ISurveyAnswerStore));
            var surveys = new List<Survey>();
            mockSurveyStore.Setup(r => r.GetRecentSurveys()).Returns(surveys);

            var result = controller.Index() as ViewResult;

            var model = result.ViewData.Model as TenantPageViewData<IEnumerable<Survey>>;
            Assert.AreSame(surveys, model.ContentModel);
        }
    }
}

