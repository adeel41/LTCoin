using System;
using System.Text;

namespace LTCoin
{
	public class Block
	{
		public Block( string data, byte[] previousBlockHash )
		{
			Timestamp = DateTime.Now.ToBinary();
			Data = Encoding.UTF8.GetBytes( data );
			PreviousBlockHash = previousBlockHash;
		}

		public long Timestamp { get; }
		public byte[] Data { get; }
		public byte[] PreviousBlockHash { get; }
		public byte[] Hash { get; private set; }
		public long Nonce { get; private set; }

		public void SetHash( long nonce, byte[] hash )
		{
			Nonce = nonce;
			Hash = hash;
		}
	}
}