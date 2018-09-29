using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.COM.Entity;

namespace Test.Console
{
    public class linq
    {
        private List<Book> bookList = new List<Book>();
        private List<Auther> autherList = new List<Auther>();
        private List<Library> libraryList = new List<Library>();

        public void Where()
        {
            //var query = from book in bookList
            //    where book.Price > 50
            //    orderby book.Sales descending,book.BookName
            //    select book;

            //var ary = new List<string>(){ "Aa", "Bb", "Cc"};

            //var a1 = ary.Where(n => n.Contains("a")).ToList();

            //ary.Add("Ga");

            //var query = from book in bookList
            //    group book by new
            //    {
            //        book.Type,
            //        book.AutherId
            //    }
            //    into bs
            //    select new
            //    {
            //        Type = bs.First().Type,
            //        AutherId = bs.First().AutherId,
            //        Count = bs.Count()
            //    };

            //query = bookList.GroupBy(n => new {n.Type, n.AutherId}).Select(g => new
            //{
            //    Type = g.First().Type,
            //    AutherId = g.First().AutherId,
            //    Count = g.Count()
            //});
            //count、sum、min、max、average、aggregate

            ////个数
            //var count = bookList.Count(n => n.Sales > 50);
            ////求和
            //var sum = bookList.Sum(n => n.Price);
            ////最小值
            //var min = bookList.Min(n => n.Sales);
            ////最大值
            //var max = bookList.Max(n => n.Price);
            ////平均值
            //var average = bookList.Average(n => n.Sales);
            ////累加,总销量
            //var aggregate1 = bookList.Select(n => n.Sales).Aggregate((g, y) => g + y);
            ////累加，初始值
            //var aggregate2 = bookList.Select(n => n.Sales).Aggregate(10, (g, y) => g + y);
            ////累加，初始值，结果处理
            //var aggregate3 = bookList.Select(n => n.Sales).Aggregate(10, (g, y) => g + y, result => result/100);

            //Book[] ary = bookList.ToArray();
            //List<Book> list = bookList.ToList();
            //Dictionary<int, Book> dic = bookList.ToDictionary<Book, int>(n => n.Id);
            ////转换成LookUp集合,key-以key分组的内部集合
            //ILookup<string, Book> look = bookList.ToLookup(n => n.Type);
            //IEnumerable<Book> cast = ary.Cast<Book>();

            //int[] ary1 = {1, 2, 2, 4, 5};
            //int[] ary2 = {3, 5, 5, 6, 10, 7};

            ////合并,自动去重
            //var union = ary1.Union(ary2);//1, 2, 3, 4, 5, 6, 7, 10
            ////合并,不会去重
            //var concat = ary1.Concat(ary2);//1, 2, 2, 4, 5, 3, 5, 5, 6, 10, 7
            ////去重
            //var distict = ary1.Distinct();//1, 2, 4, 5
            ////取交集,自动去重
            //var intersect = ary1.Intersect(ary2);//5
            ////取补集,自动去重
            //var except = ary1.Except(ary2);//1, 2, 4

            //var query = from library in libraryList
            //            from book in library.BookList
            //            where book.Type == "小说"
            //            select book;

            //query = libraryList.SelectMany(n => n.BookList).Where(g => g.Type == "小说");

            //query = libraryList.SelectMany(n => n.BookList, 
            //    (n, g) => new {n.Address, g.BookName, g.Sales})
            //    .Where(y => y.Sales > 100);

            //int[] numbers = { 1, 2, 3 };
            //string[] words = { "One", "Two", "Three", "Four" };
            ////元素依次组合，长度为较小的集合
            //IEnumerable<string> zip = numbers.Zip(words, (n, g) => n + "=" + g);//["1=One", "2=Two", "3=Three"]

            ////跳过集合的前n个元素
            //var skip = words.Skip(3);//["Four"]
            ////获取集合的前n个元素,有延迟
            //var take = numbers.Take(2);//[1, 2]

            //int pageSize = 3;//每页容量
            //int pageNum = 0;//页数
            //var page = words.Skip(pageSize * pageNum).Take(pageSize);//["One", "Two", "Three"]

            //内连接
            var query1 = from book in bookList
                join auther in autherList on book.AutherId equals auther.Id
                select new { book.BookName, auther.AutherName };
            //组连接
            var query2 = from auther in autherList
                    join book in bookList on auther.Id equals book.AutherId into items
                    select new { auther.AutherName, Books = items };
            //左外连接
            var query3 = from book in bookList
                        join auther in autherList on book.AutherId equals auther.Id into items
                        from auther in items.DefaultIfEmpty()
                        select new
                        {
                            book.BookName,
                            AutherName = auther == default(Auther) ? "无" : auther.AutherName
                        };
            //多条件连接
            var query4 = from book in bookList
                join auther in autherList on new {name = book.BookName, id = book.AutherId} equals new {name = auther.AutherName, id = auther.Id}
                select book;
        }
    }
}
