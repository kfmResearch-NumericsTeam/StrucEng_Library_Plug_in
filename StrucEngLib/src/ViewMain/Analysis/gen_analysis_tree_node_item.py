
skeleton = """
    bool? _flag%N%;
    public bool? Flag%N%
    {
        get => _flag%N%;
        set
        {
            _flag%N% = value;
            OnPropertyChanged();
        }
    }
"""

l = [
 'rf'
,'rfx'
,'rfy'
,'rfz'
,'rfm'
,'rm'
,'rmx'
,'rmy'
,'rmz'
,'rmm'
,'u'
,'ux'
,'uy'
,'uz'
,'um'
,'ur'
,'urx'
,'ury'
,'urz'
,'urm'
,'cf'
,'cfx'
,'cfy'
,'cfz'
,'cfm'
,'cm'
,'cmx'
,'cmy'
,'cmz'
,'cmm'
]


for _ in l:
    print(skeleton.replace("%N%", _))