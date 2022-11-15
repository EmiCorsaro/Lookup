
public class ResponseData<T>:ResponseSuccess where T: new()
{
    public T Data {get;set;}

    public ResponseData()
    {
        Data = new T();
    }
}