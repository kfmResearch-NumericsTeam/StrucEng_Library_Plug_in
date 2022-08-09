# === strucenglib general header for code exec

def _strucenglib_format():
    import traceback
    msg = 'StrucEngLib Plugin failed to execute code.\n'
    msg += 'Check traceback shown below and in Rhino command log. '
    msg += 'Type StrucEngLibHelp command for more help. \n\n'
    msg += traceback.format_exc()
    msg += '\n'
    print(msg)
    return msg


try:
    # == begin body ==

