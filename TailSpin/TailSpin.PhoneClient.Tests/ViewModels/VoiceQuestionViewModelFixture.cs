//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Tests.ViewModels
{
    using Microsoft.Silverlight.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TailSpin.PhoneClient.Models;
    using TailSpin.PhoneClient.ViewModels;

    [TestClass]
    [Tag("VoiceQuestionViewModelFixture")]
    public class VoiceQuestionViewModelFixture
    {
        [TestMethod]
        public void WhenCreatedIsRecordingIsFalse()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);

            Assert.IsFalse(vm.IsRecording);
        }

        [TestMethod]
        public void WhenCreatedActionTextIsStartRecording()
        {
            var expected = "Start Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);

            Assert.AreEqual(expected, vm.DefaultActionText);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndNotRecordingRecordingComesTrue()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsFalse(initialState);
            Assert.IsTrue(vm.IsRecording);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndNotRecordingActionTextComesStopRecording()
        {
            var expected = "Stop Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsFalse(initialState);
            Assert.AreEqual(expected, vm.DefaultActionText);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndRecordingRecordingComesTrue()
        {
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);
            vm.DefaultActionCommand.Execute(null);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsTrue(initialState);
            Assert.IsFalse(vm.IsRecording);
        }

        [TestMethod]
        public void WhenInvokeDefaultAndRecordingActionTextComesStartRecording()
        {
            var expected = "Start Recording";
            var answer = new QuestionAnswer();
            var vm = new VoiceQuestionViewModel(answer);
            vm.DefaultActionCommand.Execute(null);
            var initialState = vm.IsRecording;
            vm.DefaultActionCommand.Execute(null);
            Assert.IsTrue(initialState);
            Assert.AreEqual(expected, vm.DefaultActionText);
        }
    }
}
