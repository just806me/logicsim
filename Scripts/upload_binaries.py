from xml.etree import ElementTree as XmlElementTree
import subprocess
import tempfile
import requests
import hashlib
import urllib
import zlib
import os

api_url = 'http://vincad.tk/api'
temp_folder = tempfile.gettempdir()

def zip_windows(folder):
    print('Zipping windows files.')

    exefilename = os.path.join(temp_folder, 'VinCAD.Windows.exe')
    zipfilename = os.path.join(temp_folder, 'VinCAD.Windows.zip')

    subprocess.call(['7z', 'a', '-t7z', '-mmt', 
            zipfilename, 
            os.path.join(folder, '*')],
        shell=True,
        stdout=subprocess.DEVNULL)
    subprocess.call(['copy', '/b', 
            '7zS.sfx', '+', 'config.txt', '+', zipfilename, 
            exefilename],
        shell=True,
        stdout=subprocess.DEVNULL)
    
    return exefilename

def zip_unix(folder):
    print('Zipping unix files.')

    filename = os.path.join(temp_folder, 'VinCAD.Unix.7z')
    subprocess.call(['7z', 'a', '-t7z', '-mmt', 
            filename, 
            os.path.join(folder, '*.dll'), 
            os.path.join(folder, 'VinCAD.WindowsUI.exe'), 
            os.path.join(folder, 'uk')], 
        shell=True,
        stdout=subprocess.DEVNULL)

    return filename

def get_version(folder):
    print('Getting new version.')
    return XmlElementTree.parse(os.path.join(folder, 'VinCAD.WindowsUI.exe.manifest')).getroot()[3][0].attrib['version']

def get_key(file):
    print('Generating key.')

    S = 'erf5 5f44 ed5h 8exv#'

    file = open(file, 'rb')
    key = hashlib.sha1((S + str(zlib.crc32(file.read()) & 0xFFFFFFFF)).encode()).digest()[5:13]
    file.close()

    return key

def upload(args):
    print('Uploading files.')

    return requests.post(urllib.parse.urljoin(api_url, '/app/test'),
        data=
        {
            'version': args.version,
            'wauth': get_key(args.windows),
            'uauth': get_key(args.unix),
        },
        files=
        {
            'windows': open(args.windows, 'rb'),
            'unix': open(args.unix, 'rb')
        })

if __name__ == '__main__':
    from argparse import ArgumentParser

    parser = ArgumentParser()
    parser.add_argument('-v', dest='version', default=get_version(r'..\\VinCAD.WindowsUI\bin\Release'))
    parser.add_argument('-unix', dest='unix', default=zip_unix(r'..\\VinCAD.WindowsUI\bin\Release'))
    parser.add_argument('-windows', dest='windows', default=zip_windows(r'..\\Setup\Debug'))

    result = upload(parser.parse_args())
    if result.ok:
        print('Uploaded a new version.')
        input('Press any key to exit...')
    else:
        print(result.text)
        print('An error occured. The response from the server is above.')
        input('Press any key to exit...')

