namespace Rhythm
{
    public interface IResultProvider
    {
        void DisplayResult(in HeaderInformation header);
        void SaveRecordData();
    }
}