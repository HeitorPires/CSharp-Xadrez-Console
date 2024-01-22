using tabuleiro;

namespace xadrez
{
    internal class Bispo : Peca
    {
        public Bispo(Tabuleiro tab, Cor cor) : base(cor, tab)
        {
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.GetPeca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];
            Posicao pos = new(0, 0);

            //Noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
                
            }

            //Nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
            }

            //Sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
            }

            //Sudoeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.GetPeca(pos) != null && Tab.GetPeca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
            }


            return mat;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}
