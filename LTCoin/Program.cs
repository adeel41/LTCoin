using System;

namespace LTCoin
{
	internal class Program
	{
		private static void Main( string[] args )
		{
			var blockchain = BlockChain.NewBlockChainWithGenesisBlock();
			blockchain.AddBlock("Send 1 LTC to Adeel");
			blockchain.AddBlock("Send 2 more LTC to Adeel");
			foreach ( var block in blockchain.GetBlocks() )
			{
				Console.WriteLine( $"Previous Hash: {block.PreviousBlockHash.HexString()}" );
				Console.WriteLine( $"Data: {block.Data.String()}" );
				Console.WriteLine( $"Nonce: {block.Nonce}" );
				Console.WriteLine( $"Timestamp: {DateTime.FromBinary( block.Timestamp )}" );
				Console.WriteLine( $"Hash: {block.Hash.HexString()}" );
				Console.WriteLine( $"Is Valid: {blockchain.ValidateBlock( block )}\r\n" );
			}
		}
	}
}