using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

                PartidaDeXadrez partidaDeXadrez = new();
                while (!partidaDeXadrez.terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partidaDeXadrez.Tab);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                    bool[,] posicoesPossiveis = partidaDeXadrez.Tab.GetPeca(origem).MovimentosPossiveis();
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partidaDeXadrez.Tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                     
                    partidaDeXadrez.ExecutaMovimento(origem, destino);
                }

                Tela.ImprimirTabuleiro(partidaDeXadrez.Tab);
                Console.ReadKey();
            }
            catch (TabuleiroException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
