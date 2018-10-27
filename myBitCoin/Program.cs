using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace myBitCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a sample Block chain implementaion for my project");

                
            BlockChain humberCoin = new BlockChain();


            humberCoin.addBlock(new Block(1,"Sam gave rudy $100", "20/10/2018",""));
            humberCoin.addBlock(new Block(2, "Rus gave tod $50", "21/10/2018", ""));
            humberCoin.addBlock(new Block(3, "Gaurav gave Ford $150", "22/10/2018", ""));

            // Print the JASON object to the screen 
            String obj = JsonConvert.SerializeObject(humberCoin);
            Console.WriteLine(obj);

            // check the validity of the Block chain 
            Console.WriteLine("is the Block chain Valid :-" +humberCoin.isChainValid());

            Console.ReadLine();

        }
    }


    // class def of Blocks 
    class Block
    {
        // the Block property -->> Data, can include details like smart contracts, Deposits and Transfers etc ...
        int index;
        String data;
        String timeStamp;
        public String hashValue;
        public String previousHashValue;


        // always initialize the Block Object with Index, Data , TimeStamp and PreviousHashValue 
        // However dont set the hash via constructor !! it has to be set via a function GenerateMD5Hash
        public Block(int index, String data, String timeStamp, String previousHashValue )
        {
            this.index = index;     // what should i do with the index here ? 
            this.data = data;
            this.timeStamp = timeStamp;
            this.previousHashValue = previousHashValue;

            // Generate hash value based on the the function; 
            this.hashValue = GenerateMd5HashValue();

        }


        //this function generates a Hash value for  a combination of Index, Data , TimeStamp and PreviousHashValue
        //MD5 hashing is old; try SHA256 algorithm with is new and secure; 
        public string GenerateMd5HashValue()
        {
            MD5 md5Hash = MD5.Create();
            
            // Create MD5 hash  array 
            byte[] hashArray = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(index.ToString() + data +timeStamp +previousHashValue));
            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hashArray.Length; i++)
            {
                strBuilder.Append(hashArray[i].ToString("x2"));
            }
            
            //return hexadecimal string 
            return strBuilder.ToString();
        }

    }

    //Class def of BlockChain class; it basically contains a list of Block Objects, chained together; 
    class BlockChain
    {

        // Declaring an array of block; A list is preferable as the size of  chain is not known
        // Need to convert it into list 
        //Block[] myBlockChain = new Block[1000];

        //Try Array List
        List<Block> myBlockChain = new List<Block>();

        public BlockChain()
        {
            // Assign Genisys Block as the First Block of this chain; 
            myBlockChain.Add(CreateGenisysBlock());
        }

        // Create a genisys block or the First Block; 
        Block CreateGenisysBlock()
        {

            Block genBlock = new Block(0,"This is Genisis Block","15/10/2018","") ;
            return genBlock;

        }

        // !!!!IMPORTANT!!!  There is no delete method for Blocks! This is ADD only Public Ledger of Blocks
        public void addBlock(Block newBlock)
        {
            //Set the previousHashValue of the new Block to point to the latest Block in the Block chain;  
            newBlock.previousHashValue = this.getLatestBlock().hashValue;

            //Recalculate the hash of the new block because the previousHashValue has been updated
            newBlock.hashValue = newBlock.GenerateMd5HashValue();

            //push the new block to the chain 
            myBlockChain.Add(newBlock);

            // !!IMPORTANT!! A new Block cannot be added so easily, it has to go throgh some checks and balances  


        }

        public Block getLatestBlock()
        {
            // BUG this is a Array and the lenght is fixed i.e its 1000; 
            // Need to convert it to List ! 
            // Need to confirm the index of the count 

            return myBlockChain[myBlockChain.Count - 1];

            //return myBlockChain[myBlockChain.FindLastIndex];

        }



        // House Keeping functions to check the validity of the block 
        public Boolean isChainValid()
        {
            for (int i = 1; i < myBlockChain.Count; i++)
            {
                Block currentBlock = this.myBlockChain[i];
                Block previosBlock = this.myBlockChain[i-1];

                if (currentBlock.hashValue != currentBlock.GenerateMd5HashValue())
                {
                    return false;
                }

                if (previosBlock.hashValue != currentBlock.previousHashValue)
                {
                    return false;
                }
            }

            return true;

        }

    }




}
