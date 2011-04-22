namespace TailSpin.PhoneClient.Models
{
    public enum QuestionType
    {
        SimpleText,
        MultipleChoice,
        FiveStars,
        Picture,
        Voice
    }

    public static class QuestionTypeParser
    {
        public static QuestionType Parse(string p)
        {
            switch (p)
            {
                case "SimpleText":
                    return QuestionType.SimpleText;
                case "MultipleChoice":
                    return QuestionType.MultipleChoice;
                case "FiveStars":
                    return QuestionType.FiveStars;
                case "Picture":
                    return QuestionType.Picture;
                case "Voice":
                    return QuestionType.Voice;
                default:
                    return QuestionType.SimpleText;
            }
        }
    }
}
