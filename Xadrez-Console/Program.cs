using tabuleiro;

namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab = new(8, 8);

            Tela.ImprimirTabuleiro(tab);
            Console.ReadKey();
        }
    }
}
