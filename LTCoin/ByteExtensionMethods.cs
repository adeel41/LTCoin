using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LTCoin
{
	public static class ByteExtensionMethods
	{
		public static string String( this byte[] bytes ) { return Encoding.UTF8.GetString( bytes ); }
		public static string HexString( this byte[] bytes ) { return BitConverter.ToString( bytes ).Replace( "-", "" ); }

		public static byte[] Join( this byte[] instance, params byte[][] bytes )
		{
			var data = new byte[instance.Length + bytes.Sum( x => x.Length )];
			Buffer.BlockCopy( instance, 0, data, 0, instance.Length );
			var currentOffset = instance.Length;
			foreach ( var b in bytes )
			{
				Buffer.BlockCopy( b, 0, data, currentOffset, b.Length );
				currentOffset += b.Length;
			}

			return data;
		}

		public static int CompareSequenceTo( this IEnumerable<byte> a, IEnumerable<byte> b )
		{
			return a
				.Zip( b, ( x, y ) => x.CompareTo( y ) )
				.FirstOrDefault( r => r != 0 );
		}
	}
}