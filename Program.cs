using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace TicTacToeGame
{

    public enum Player { USER, COMPUTER };
    public enum GameStatus { WON, FULL_BOARD, CONTINUE };
    class TicTacToe
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tic Tac Toe Game!");
            char[] board = CreateBoard();
            showBoard(board);

            char userLetter = chooseInput(board);
            char compLetter = 'X';
            if (userLetter.Equals('X'))
            {
                compLetter = 'O';
            }
            Player p = getHeadTail();
            bool play = true;
            GameStatus gstatus;
            while (play)
            {
                if (p == Player.USER)
                {
                    showBoard(board);
                    int userMove = getBoxNum(board, userLetter);
                    String mess = "You won";
                    gstatus = CheckCurrentStatus(board, userMove, userLetter, mess);
                    p = Player.COMPUTER;
                }
                else
                {
                    showBoard(board);
                    int compMove = getCompMove(board, compLetter, userLetter);
                    //showBoard(board);
                    String mess = "Computer won";
                    gstatus = CheckCurrentStatus(board, compMove, compLetter, mess);
                    p = Player.USER;
                }

                if (gstatus == GameStatus.CONTINUE) continue;
                play = false;
            }

        }
        public static char[] CreateBoard()
        {
            char[] board = new char[10];
            for (int i = 0; i < 10; i++)
            {
                board[i] = ' ';
            }
            Console.WriteLine("Board Created");
            return board;
        }
        /*UC2*/
        public static char chooseInput(char[] board)
        {
            char input = 'X';
            bool val = true;
            while (val)
            {
                Console.WriteLine("Choose X or O");
                input = (Console.ReadLine()[0]);
                if (char.ToUpper(input) == 'X' | char.ToUpper(input) == 'O')
                {
                    Console.WriteLine("You chose " + char.ToUpper(input));
                    val = false;
                    //getBoxNum(board, char.ToUpper(input));
                }
                else
                    Console.WriteLine("Invalid Input");
            }
            return char.ToUpper(input);
        }
        /*UC3*/
        public static void showBoard(char[] board)
        {
            Console.WriteLine("\n " + "|" + board[0] + "|" + board[1] + "|" + board[2] + "|");
            Console.WriteLine("-------");
            Console.WriteLine(" " + "|" + board[3] + "|" + board[4] + "|" + board[5] + "|");
            Console.WriteLine("-------");
            Console.WriteLine(" " + "|" + board[6] + "|" + board[7] + "|" + board[8] + "|");
        }
        /*UC5*/
        public static void Play(char[] board, int pos, char pLetter)
        {
            board[pos] = pLetter;
        }
        /*UC4*/
        public static int getBoxNum(char[] board, char userinput)
        {
            int index = 0;
            int[] box = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            try
            {
                Console.WriteLine("Where do you wanna enter(1-9): ");
                index = Convert.ToInt32(Console.ReadLine());
                if (board[index - 1] == ' ')
                {
                    return index - 1;
                }
                else
                {
                    Console.WriteLine("Index already taken. Try Again");
                    return index - 1;
                }
            }
            catch
            {
                Console.WriteLine("Invalid Input. Enter integer from 1-9");
                return getBoxNum(board, userinput);
            }
        }

        public static Player getHeadTail()
        {
            Random random = new Random();
            int toss = random.Next(0, 2);
            if (toss == 0)
            {
                Console.WriteLine("You go first");
                return Player.USER;
            }
            else
            {
                Console.WriteLine("Computer goes first");
                return Player.COMPUTER;
            }
        }
        public static bool isWinner(char[] b, char ch)
        {
            return ((b[0] == ch && b[1] == ch && b[2] == ch) ||  //top row
                    (b[3] == ch && b[4] == ch && b[5] == ch) ||  //middle row
                    (b[6] == ch && b[7] == ch && b[8] == ch) ||  //bottom row
                    (b[0] == ch && b[3] == ch && b[6] == ch) ||  //first column
                    (b[1] == ch && b[4] == ch && b[7] == ch) ||  //second column
                    (b[2] == ch && b[5] == ch && b[8] == ch) ||  //third column
                    (b[0] == ch && b[4] == ch && b[8] == ch) ||  //first diagonal
                    (b[2] == ch && b[4] == ch && b[6] == ch)     //second diagonal
                );
        }
        /*UC 8/9/10/11*/
        public static int getCompMove(char[] board, char compLetter, char userLetter)
        {
            int compwin = getWinningMove(board, compLetter);
            if (compwin != 0) return compwin;
            int compblock = getWinningMove(board, userLetter);
            if (compblock != 0) return compblock;
            int[] cornerMoves = { 0, 2, 6, 8 };
            Random random = new Random();
            int cornMove = random.Next(0, 4);
            if (cornMove != 0) return cornerMoves[cornMove];
            if (board[4] == ' ') return 4;
            int[] sideMoves = { 1, 3, 5, 7 };
            int sideindex = random.Next(0, 4);
            if (sideindex != 0) return sideMoves[sideindex];
            return 0;
        }
        public static int getWinningMove(char[] board, char letter)
        {
            for (int i = 0; i < 9; i++)
            {
                char[] copyBoard = BoardCopy(board);
                if (copyBoard[i] == ' ')
                {
                    Play(copyBoard, i, letter);
                    if (isWinner(board, letter))
                        return i;
                }
            }
            //showBoard(board);
            return 0;
        }
        public static char[] BoardCopy(char[] board)
        {
            char[] boardCopy = new char[10];
            Array.Copy(board, 0, boardCopy, 0, board.Length);

            return boardCopy;
        }
        /*UC12*/
        public static GameStatus CheckCurrentStatus(char[] board, int move, char Letter, String mess)
        {
            Play(board, move, Letter);
            bool isFull = IsBoardFull(board);
            if (isFull)
            {
                showBoard(board);
                Console.WriteLine("Tie");
                return GameStatus.FULL_BOARD;
            }
            if (isWinner(board, Letter))
            {
                showBoard(board);
                Console.WriteLine(mess);
                return GameStatus.WON;
            }
            return GameStatus.CONTINUE;
        }
        public static bool IsBoardFull(char[] board)
        {
            bool val = true;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == ' ')
                {
                    val = false;
                }
            }
            return val;
        }

    }
}
