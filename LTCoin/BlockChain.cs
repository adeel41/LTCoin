using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace LTCoin
{
	public class BlockChain
	{
		public static int TargetBits = 16;
		private List<Block> _blocks { get; } = new List<Block>();

		public void AddBlock( string data )
		{
			var previousBlock = _blocks[_blocks.Count - 1];
			var newBlock = NewBlock( data, previousBlock.Hash );
			_blocks.Add( newBlock );
		}

		public IReadOnlyCollection<Block> GetBlocks() { return _blocks.AsReadOnly(); }

		private static Block NewBlock( string data, byte[] previousBlockHash )
		{
			var block = new Block( data, previousBlockHash );
			var proofOfWork = new ProofOfWork( block, GetTargetBytes() );
			var runResult = proofOfWork.Run();
			block.SetHash( runResult.Nonce, runResult.Hash );
			return block;
		}

		private static Block NewGenesisBlock() { return NewBlock( "Genesis Block", new byte[0] ); }

		public static BlockChain NewBlockChainWithGenesisBlock()
		{
			var blockchain = new BlockChain();
			blockchain._blocks.Add( NewGenesisBlock() );
			return blockchain;
		}

		private static byte[] GetTargetBytes()
		{
			var target = new BigInteger( Math.Pow( 2, 256 - TargetBits ) );
			var targetBytes = new byte[32];
			target.ToByteArray().CopyTo( targetBytes, 0 );
			targetBytes = targetBytes.Reverse().ToArray();
			return targetBytes;
		}

		public bool ValidateBlock( Block block ) => new ProofOfWork( block, GetTargetBytes() ).Validate();
	}
}