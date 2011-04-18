




 




namespace TailSpin.Web.Survey.Public.Utility
{
    using Shared.Models;

    public static class QuestionTemplateFactory
    {
        public static string Create(QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.SimpleText:
                    return "SimpleText";
                case QuestionType.MultipleChoice:
                    return "MultipleChoice";
                case QuestionType.FiveStars:
                    return "FiveStar";
                case QuestionType.Picture:
                    return "Picture";
                case QuestionType.Voice:
                    return "Voice";
                default:
                    return "String";
            }
        }
    }
}