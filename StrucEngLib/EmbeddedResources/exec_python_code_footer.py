# === strucenglib general footer for code exec

    print('[strucenglib/codeexec]: Execution finished.')
except Exception as e:
    print('[strucenglib/codeexec]: Execution failed with exception.')
    raise Exception(_strucenglib_format())
    
    