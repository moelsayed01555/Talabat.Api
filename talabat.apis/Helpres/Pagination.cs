using talabat.apis.DTOs;

namespace talabat.apis.Helpres
{
    public class Pagination<T>
    {
       

        // To determine the structue of data of pagination (شكل الداتا اللي راجعة في الباجيناشن)

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
        }
    }
}
