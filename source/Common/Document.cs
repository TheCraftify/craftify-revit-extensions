namespace Craftify.Revit.Extensions.Common;

public static partial class DocumentExtensions
{
    public static DirectShape CreateDirectShape(
        this Document document,
        IList<GeometryObject> geometryObjects,
        BuiltInCategory builtInCategory = BuiltInCategory.OST_GenericModel
    )
    {
        var directShape = DirectShape.CreateElement(document, new ElementId(builtInCategory));
        directShape.AppendShape(geometryObjects);
        return directShape;
    }

    public static void CreateSingleTransaction(
        this Document document,
        string name,
        Action action,
        Func<FailuresAccessor, FailureProcessingResult>? preprocessFailures = default
    )
    {
        using var transaction = new Transaction(document, name);

        if (preprocessFailures is not null)
        {
            var failuredHandlingOptions = transaction.GetFailureHandlingOptions();
            var failuresPreprocessor = FailuresPreprocessor.Create(preprocessFailures);
            failuredHandlingOptions.SetFailuresPreprocessor(failuresPreprocessor);
            transaction.SetFailureHandlingOptions(failuredHandlingOptions);
        }

        transaction.Start();
        action();
        transaction.Commit();
    }

    public static T CreateSingleTransaction<T>(
        this Document document,
        string name,
        Func<T> function,
        Func<FailuresAccessor, FailureProcessingResult>? preprocessFailures = default
    )
    {
        using var transaction = new Transaction(document, name);

        if (preprocessFailures is not null)
        {
            var failuredHandlingOptions = transaction.GetFailureHandlingOptions();
            var failuresPreprocessor = FailuresPreprocessor.Create(preprocessFailures);
            failuredHandlingOptions.SetFailuresPreprocessor(failuresPreprocessor);
            transaction.SetFailureHandlingOptions(failuredHandlingOptions);
        }

        transaction.Start();
        var result = function();
        transaction.Commit();

        return result;
    }

    public static IEnumerable<RevitLinkInstance> AllLoadedRevitLinkInstances(
        this Document document
    ) =>
        new FilteredElementCollector(document)
            .OfClass(typeof(RevitLinkInstance))
            .WhereElementIsNotElementType()
            .OfType<RevitLinkInstance>()
            .Where(revitLinkInstance =>
                RevitLinkType.IsLoaded(document, revitLinkInstance.GetTypeId())
            );

    public static IEnumerable<Document> AllLoadedRevitLinkDocuments(this Document document) =>
        document
            .AllLoadedRevitLinkInstances()
            .Select(revitLinkInstance => revitLinkInstance.GetLinkDocument());

    public static void CreateTransactionGroup(
        this Document document,
        string name,
        params Action<InnerTransaction>[] innerTransactions
    )
    {
        using var transactionGroup = new TransactionGroup(document, name);

        transactionGroup.Start();
        foreach (var innerTransaction in innerTransactions)
        {
            var innerTransactionBuilder = new InnerTransaction(document);
            innerTransaction?.Invoke(innerTransactionBuilder);
        }
        transactionGroup.Assimilate();
    }
}

public class InnerTransaction(Document document)
{
    private string _name = "InnerTransaction";
    private Func<FailuresAccessor, FailureProcessingResult>? _preprocessFailures = default;

    public InnerTransaction Name(string name)
    {
        _name = name;
        return this;
    }

    public InnerTransaction PreprocessFailures(
        Func<FailuresAccessor, FailureProcessingResult>? preprocessFailures = default
    )
    {
        _preprocessFailures = preprocessFailures;
        return this;
    }

    public InnerTransaction Execute(Action action)
    {
        document.CreateSingleTransaction(_name, action, _preprocessFailures);
        return this;
    }

    public T Execute<T>(Func<T> function) =>
        document.CreateSingleTransaction(_name, function, _preprocessFailures);
}
