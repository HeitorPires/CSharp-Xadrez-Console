using System;
using tabuleiro;
using xadrez;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public bool terminada { get; private set; }
        public  int Turno { get; private set; }
        public Cor JogadorAtual { get; private set;}

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

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            Tab.ValidarPosicao(pos);

            if (Tab.GetPeca(pos) == null)
                throw new TabuleiroException("Não existe peca na posicão de origem escolhida!");

            if(JogadorAtual != Tab.GetPeca(pos).Cor)
                    throw new TabuleiroException("A peca de origem escolhida não é sua!");

            if (!Tab.GetPeca(pos).ExisteMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peca de origem escolhida");
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.GetPeca(origem).PodeMoverPara(destino))
                throw new TabuleiroException("Posicão de destino inválida");
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        private void ColocarPecas()
        {
            //Brancas
            Tab.ColocarPeca(new Rei(Tab, Cor.Branca), new PosicaoXadrez('d', 1).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Branca), new PosicaoXadrez('a', 1).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Branca), new PosicaoXadrez('h', 1).ToPosicao());

            //Pretas
            Tab.ColocarPeca(new Rei(Tab, Cor.Preta), new PosicaoXadrez('e',8).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preta), new PosicaoXadrez('a',8).ToPosicao());
            Tab.ColocarPeca(new Torre(Tab, Cor.Preta), new PosicaoXadrez('h',8).ToPosicao());
        }
    }
}
