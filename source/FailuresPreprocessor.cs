namespace Craftify.Revit.Extensions;

public static class FailuresPreprocessor
{
    public static IFailuresPreprocessor Create(
        Func<FailuresAccessor, FailureProcessingResult> preprocessFailures
    ) => new TransactionFailuresPreprocessor(preprocessFailures);

    class TransactionFailuresPreprocessor(
        Func<FailuresAccessor, FailureProcessingResult> preprocessFailures
    ) : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor) =>
            preprocessFailures(failuresAccessor);
    }
}
