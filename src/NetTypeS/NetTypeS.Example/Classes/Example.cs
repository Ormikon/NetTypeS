using System;
using NetTypeS.Example.Enums;
using NetTypeS.Example.Interfaces;

namespace NetTypeS.Example.Classes
{
	public class Example : IExample
	{
		private int simpleInt;
		private string simpleString;
		private bool simpleBoolean;
		private DateTime simpleDate;
		private FirstEnum firstEnum;
		private SecondEnum secondEnum;

		public int SimpleInt
		{
			get { return simpleInt; }
			set { simpleInt = value; }
		}

		public string SimpleString
		{
			get { return simpleString; }
			set { simpleString = value; }
		}

		public bool SimpleBoolean
		{
			get { return simpleBoolean; }
			set { simpleBoolean = value; }
		}

		public DateTime SimpleDate
		{
			get { return simpleDate; }
			set { simpleDate = value; }
		}

		public FirstEnum FirstEnum
		{
			get { return firstEnum; }
			set { firstEnum = value; }
		}

		public SecondEnum SecondEnum
		{
			get { return secondEnum; }
			set { secondEnum = value; }
		}
	}
}
