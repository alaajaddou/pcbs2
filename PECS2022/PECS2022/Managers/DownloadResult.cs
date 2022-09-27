using System.Collections.Generic;

namespace PECS2022.Managers
{
    public class DownloadResult<T> where T : class
    {

        public DownloadResult(bool isSuccess, List<T> list)
        {
            this.IsSuccess = isSuccess;
            this.Data = list;
        }

        public DownloadResult(bool isSuccess, T value)
        {
            this.IsSuccess = isSuccess;
            this.Value = value;
        }

        public bool IsSuccess { get; private set; }
        public List<T> Data { get; private set; }

        public T Value { get; private set; }

    }
}