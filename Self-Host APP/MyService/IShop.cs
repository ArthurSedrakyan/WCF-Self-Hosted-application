using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyService
{
    [ServiceContract]
    public interface IShop
    {
        [OperationContract]
        Result Add(Book book);

        [OperationContract]
        Result UpdatePrice(int id, int price);

        [OperationContract]
        List<Book> SeeAll();
    }

    [DataContract]
    public class Book
    {
        private int id;
        private string author;
        private string title;
        private double price;
        private int year;

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [DataMember]
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
    }

    [DataContract]
    public struct Result
    {
        public Result(string messege, string answer)
        {
            this.messege = messege;
            this.answer = answer;
        }

        private string messege;
        private string answer;

        [DataMember]
        public string Messege
        {
            get { return messege; }
            set { messege = value; }
        }

        [DataMember]
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
    }
}
