




 




namespace TailSpin.Web.Survey.Shared.Stores
{
    using System.IO;
    using TailSpin.Web.Survey.Shared.Models;

    public interface IMediaAnswerStore
    {
        void Initialize();
        string SaveMediaAnswer(Stream media, QuestionType questionType);
        void DeleteMediaAnswer(string id, QuestionType questionType);
    }
}