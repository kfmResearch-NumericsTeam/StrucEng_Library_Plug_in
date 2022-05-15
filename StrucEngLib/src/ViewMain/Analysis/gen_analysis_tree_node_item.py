
skeleton = """
    private bool? _flag_%N% = false;
    public bool? Flag_%N%
    {
        get => _flag_%N%;
        set
        {
            _flag_%N% = value;
            OnPropertyChanged();
        }
    }
"""

init = """Flag_%N% = false;"""

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


for _ in l:
    print(init.replace("%N%", _))
