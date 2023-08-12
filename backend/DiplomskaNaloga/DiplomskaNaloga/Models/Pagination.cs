using System;
namespace DiplomskaNaloga.Models
{
	public class Pagination<T>
	{
		public List<T> Items { get; set; }
		public int Count { get; set; }
		public int Page { get; set; }
		public int Size { get; set; }
	}
}

