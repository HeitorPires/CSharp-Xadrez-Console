using System;
using tabuleiro;
using xadrez;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public bool terminada {  get; private set; }
        private int Turno;
        private Cor JogadorAtual;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            ColocarPecas();
            terminada = false;
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = Tab.RetirarPeca(origem);
            pecaOrigem.IncrementarQteMovimentos();
            Peca pecaCapturada =Tab.RetirarPeca(destino);
            Tab.ColocarPeca(pecaOrigem, destino);
        }

        private void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(Tab, Cor.Preta), new PosicaoXadrez('a',1).ToPosicao());
            Tab.ColocarPeca(new Rei(Tab, Cor.Branca), new PosicaoXadrez('h', 8).ToPosicao());
        }
    }
}
