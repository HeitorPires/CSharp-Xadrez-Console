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
        private HashSet<Peca> Pecas;
        private HashSet<Peca> PecasCapturadas;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            terminada = false;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = Tab.RetirarPeca(origem);
            pecaOrigem.IncrementarQteMovimentos();
            Peca pecaCapturada =Tab.RetirarPeca(destino);
            Tab.ColocarPeca(pecaOrigem, destino);
            if (pecaCapturada != null)
                PecasCapturadas.Add(pecaCapturada);
            
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

        public HashSet<Peca> PecasCapturadasDeCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca> ();
            foreach (Peca peca in PecasCapturadas)
            {
                if (peca.Cor == cor)
                    aux.Add(peca);
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogoDeCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in Pecas)
            {
                if (peca.Cor == cor)
                    aux.Add(peca);
            }
            aux.ExceptWith(PecasCapturadasDeCor(cor));
            return aux;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            //Brancas
            ColocarNovaPeca('d', 1, new Rei(Tab, Cor.Branca));
            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));

            //Pretas
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta));
            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
        }
    }
}
