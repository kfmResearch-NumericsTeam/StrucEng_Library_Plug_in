namespace StrucEngLib.Model.Sm
{
    /// <summary>Properties for Analysis for Sandwich Model</summary>
    public class SmAnalysisSetting
    {
        public bool Include { get; set; }
        public Step Step { get; set; }
        public string AxesScale { get; set; }
        public string DruckzonenIteration { get; set; }
        public string MindestBewehrung { get; set; }
        public string Code { get; set; }
        public string Schubnachweis { get; set; }
        
        public bool AsXiBot { get; set; }
        public bool AsXiTop { get; set; }
        public bool AsEtaBot { get; set; }
        public bool AsEtaTop { get; set; }
        public bool AsZ { get; set; }
        public bool CCBot { get; set; }
        public bool CCTop { get; set; }
        public bool KBot { get; set; }
        public bool KTop { get; set; }
        public bool TBot { get; set; }
        public bool TTop { get; set; }
        public bool PsiBot { get; set; }
        public bool PsiTop { get; set; }
        public bool FallBot { get; set; }
        public bool FallTop { get; set; }
        public bool MCcBot { get; set; }
        public bool MCcTop { get; set; }
        public bool MShearC { get; set; }
        public bool MCTotal { get; set; }
    }
}