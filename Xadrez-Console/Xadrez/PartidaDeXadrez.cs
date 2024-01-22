using System;
using System.ComponentModel;
using tabuleiro;
using xadrez;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public bool Terminada { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }
        private HashSet<Peca> PecasEmJogo;
        private HashSet<Peca> PecasCapturadas;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            PecasEmJogo = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaOrigem = Tab.RetirarPeca(origem);
            pecaOrigem.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(pecaOrigem, destino);
            if (pecaCapturada != null)
                PecasCapturadas.Add(pecaCapturada);

            // #Jogadaespecial roque pequeno
            if (pecaOrigem is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new(origem.Linha, origem.Coluna + 1);
                Peca torre = Tab.RetirarPeca(origemTorre);
                torre.IncrementarQteMovimentos();
                Tab.ColocarPeca(torre, destinoTorre);
            }

            // #Jogadaespecial roque grande
            if (pecaOrigem is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new(origem.Linha, origem.Coluna - 1);
                Peca torre = Tab.RetirarPeca(origemTorre);
                torre.IncrementarQteMovimentos();
                Tab.ColocarPeca(torre, destinoTorre);
            }

            // #Jogadaespecial EnPassant
            if(pecaOrigem is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posPeao;
                    if(pecaOrigem.Cor == Cor.Branca)
                        posPeao = new(destino.Linha + 1, destino.Coluna);
                    else
                        posPeao = new(destino.Linha - 1, destino.Coluna);
                    pecaCapturada = Tab.RetirarPeca(posPeao);
                    PecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;

        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca peca = Tab.RetirarPeca(destino);
            peca.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                PecasCapturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(peca, origem);

            // #Jogadaespecial roque pequeno
            if (peca is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemTorre = new(origem.Linha, origem.Coluna + 3);
                Posicao destinoTorre = new(origem.Linha, origem.Coluna + 1);
                Peca torre = Tab.RetirarPeca(destinoTorre);
                torre.DecrementarQteMovimentos();
                Tab.ColocarPeca(torre, origemTorre);
            }

            // #Jogadaespecial roque grande
            if (peca is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemTorre = new(origem.Linha, origem.Coluna - 4);
                Posicao destinoTorre = new(origem.Linha, origem.Coluna - 1);
                Peca torre = Tab.RetirarPeca(destinoTorre);
                torre.IncrementarQteMovimentos();
                Tab.ColocarPeca(torre, origemTorre);
            }

            // #Jogadaespecial EnPassant
            if (peca is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (peca.Cor == Cor.Branca)
                        posP = new(3, destino.Coluna);
                    else
                        posP = new(4, destino.Coluna);

                    Tab.ColocarPeca(peao, posP);
                }
            }

        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmXeque(CorAdversaria(JogadorAtual)))
                Xeque = true;
            else
                Xeque = false;

            if (TesteXequemate(CorAdversaria(JogadorAtual)))
                Terminada = true;
            else
            {
                Turno++;
                MudaJogador();
            }

            // #Jogadaespecial EnPassant
            Peca peca = Tab.GetPeca(destino);
            if(peca is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
                VulneravelEnPassant = peca;
            
        }

        public void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
                JogadorAtual = Cor.Preta;
            else
                JogadorAtual = Cor.Branca;
        }

        private Cor CorAdversaria(Cor cor)
        {
            if (cor == Cor.Branca)
                return Cor.Preta;
            return Cor.Branca;
        }

        private Peca GetRei(Cor cor)
        {
            foreach (Peca peca in PecasEmJogo)
            {
                if (peca is Rei && peca.Cor == cor)
                    return peca;
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca rei = GetRei(cor) ?? throw new TabuleiroException($"Não existe rei da cor {cor} no tabuleiro!");
            foreach (Peca peca in PecasEmJogoDeCor(CorAdversaria(cor)))
            {
                bool[,] movimentosPossiveis = peca.MovimentosPossiveis();
                if (movimentosPossiveis[rei.Posicao.Linha, rei.Posicao.Coluna])
                    return true;
            }
            return false;
        }
        
        public bool TesteXequemate(Cor cor)
        {
            if (!EstaEmXeque(cor))
                return false;
            foreach (Peca peca in PecasEmJogoDeCor(cor))
            {
                bool[,] movimentosPossiveis = peca.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (movimentosPossiveis[i, j])
                        {
                            Posicao origem = peca.Posicao;
                            Posicao destino = new(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public HashSet<Peca> PecasCapturadasDeCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
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
            foreach (Peca peca in PecasEmJogo)
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
            PecasEmJogo.Add(peca);
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            Tab.ValidarPosicao(pos);

            if (Tab.GetPeca(pos) == null)
                throw new TabuleiroException("Não existe peca na posicão de origem escolhida!");

            if (JogadorAtual != Tab.GetPeca(pos).Cor)
                throw new TabuleiroException("A peca de origem escolhida não é sua!");

            if (!Tab.GetPeca(pos).ExisteMovimentosPossiveis())
                throw new TabuleiroException("Não há movimentos possíveis para a peca de origem escolhida");
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            Tab.ValidarPosicao(destino);

            if (!Tab.GetPeca(origem).MovimentoPossivel(destino))
                throw new TabuleiroException("Posicão de destino inválida");
        }
        
        private void ColocarPecas()
        {
            //Brancas
            ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca, this));

            ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));

            //Pretas
            ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta, this));

            ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
        }
    }
}
