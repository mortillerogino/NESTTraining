namespace NuSearch.Commands
{
    public interface IElasticSearchCommand
    {
        void CreateIndex();
        void DeleteIndexIfExists();
        void InsertDocuments();
    }
}