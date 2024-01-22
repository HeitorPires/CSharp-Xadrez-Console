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
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partidaDeXadrez.Tab);
                        Console.WriteLine();
                        Console.WriteLine($"Turno: {partidaDeXadrez.Turno}");
                        Console.WriteLine($"Aguardando jogada: {partidaDeXadrez.JogadorAtual}");

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partidaDeXadrez.validarPosicaoDeOrigem(origem);
                        bool[,] posicoesPossiveis = partidaDeXadrez.Tab.GetPeca(origem).MovimentosPossiveis();
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partidaDeXadrez.Tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partidaDeXadrez.ValidarPosicaoDestino(origem, destino);

                        partidaDeXadrez.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroException ex) 
                    { 
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Pressione qualquer tecla para continuar");
                        Console.ReadKey();
                    }
                }
            }
            catch (TabuleiroException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
