




 




namespace TailSpin.Web.Survey.Shared.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class QuestionAnswerValidatorAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var questionAnswer = value as QuestionAnswer;

            if (questionAnswer == null)
            {
                return base.IsValid(value);
            }

            if (questionAnswer.QuestionType == QuestionType.Picture ||
                questionAnswer.QuestionType == QuestionType.Voice)
            {
                return true;
            }

            return base.IsValid(value);
        }
    }
}