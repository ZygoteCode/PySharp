import os.path
import os
from os import path
import sys
import glob

pySharpDirectory = os.getenv("SystemDrive") + "\\pysharp"
currentSessionId = ""

def writeMessage(content: str):
    if not path.exists(pySharpDirectory):
        os.mkdir(pySharpDirectory)
    for i in range(1, 100):
        try:
            strActual = str(i)
            if not path.exists(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_py_to_csharp"):
                f = open(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_py_to_csharp", "w")
                f.write(content)
                f.close()
                break
        except:
            pass

def readMessage() -> str:
    if not path.exists(pySharpDirectory):
        os.mkdir(pySharpDirectory)
        return ""
    while True:
        for i in range(1, 100):
            try: 
                strActual = str(i)
                if path.exists(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_csharp_to_py"):
                    f = open(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_csharp_to_py", "r")
                    content = f.read()
                    f.close()
                    os.remove(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_csharp_to_py")
                    return content
            except:
                pass

def clearSession():
    if not path.exists(pySharpDirectory):
        os.mkdir(pySharpDirectory)
        return
    for i in range(1, 100):
        strActual = str(i)
        try:
            if path.exists(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_py_to_csharp"):
                os.remove(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_py_to_csharp")
        except:
            pass
        try:
            if path.exists(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_csharp_to_py"):
                os.remove(pySharpDirectory + "\\pysharp_" + currentSessionId + "_message_" + strActual + "_from_csharp_to_py")
        except:
            pass          

currentSessionId = sys.argv[1]
clearSession()

while True:
    print(readMessage())