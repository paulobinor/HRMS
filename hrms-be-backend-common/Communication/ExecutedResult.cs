namespace hrms_be_backend_common.Communication
{
    public class ExecutedResult<T>
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public T data { get; set; }

    }
}
