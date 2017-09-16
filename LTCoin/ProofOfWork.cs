using System;
using System.Linq;
using System.Security.Cryptography;

namespace LTCoin
{
	public class ProofOfWork
	{
		public class RunResult
		{
			public long Nonce { get; set; }
			public byte[] Hash { get; set; }
		}

		public ProofOfWork( Block block, byte[] target )
		{
			Block = block;
			Target = target;
		}

		public Block Block { get; }
		public byte[] Target { get; }

		private byte[] PrepareData( long nonce )
		{
			var timestampBytes = BitConverter.GetBytes( Block.Timestamp );
			var targetBitsBytes = BitConverter.GetBytes( BlockChain.TargetBits );
			var nonceBytes = BitConverter.GetBytes( nonce );
			var data = new byte[Block.PreviousBlockHash.Length + Block.Data.Length + timestampBytes.Length + targetBitsBytes.Length + nonceBytes.Length];
			Buffer.BlockCopy( Block.PreviousBlockHash, 0, data, 0, Block.PreviousBlockHash.Length );
			Buffer.BlockCopy( Block.Data, 0, data, Block.PreviousBlockHash.Length, Block.Data.Length );
			Buffer.BlockCopy( timestampBytes, 0, data, Block.PreviousBlockHash.Length + Block.Data.Length, timestampBytes.Length );
			Buffer.BlockCopy( targetBitsBytes, 0, data, Block.PreviousBlockHash.Length + Block.Data.Length + timestampBytes.Length, targetBitsBytes.Length );
			Buffer.BlockCopy( nonceBytes, 0, data, Block.PreviousBlockHash.Length + Block.Data.Length + timestampBytes.Length + targetBitsBytes.Length, nonceBytes.Length );
			return data;
		}

		public RunResult Run()
		{
			var result = new RunResult();
			var hashAlgorithm = SHA256.Create();
			Console.WriteLine( $"Mining the block containing {Block.Data.String()}" );
			try
			{
				for ( long nonce = 0; nonce < long.MaxValue; nonce++ )
				{
					var data = PrepareData( nonce );
					var hash = hashAlgorithm.ComputeHash( data );
					if ( hash.CompareSequenceTo( Target ) == -1 )
					{
						result.Nonce = nonce;
						result.Hash = hash;
						break;
					}
				}
			}
			finally
			{
				hashAlgorithm.Dispose();
			}

			return result;
		}

		public bool Validate()
		{
			var data = PrepareData( Block.Nonce );
			using ( var sha = SHA256.Create() )
			{
				var hash = sha.ComputeHash( data );
				return hash.CompareSequenceTo( Target ) == -1;
			}
		}
	}
}