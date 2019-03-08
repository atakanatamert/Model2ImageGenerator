import cv2
import os
import numpy as np
from operator import itemgetter
from tqdm import tqdm

arr = []
progressbar = 0
f = open("ImageFileInfo.txt", "w")
f.write("/////////////////////////////////////\n")
f.write("xMax   xMin   yMax   yMin   Image   \n")
f.write("-------------------------------------\n")

try:
    for file in os.listdir("/Users/AtakanAtamert/Desktop/Screenshots/"):
       print("\n Detecting edge location for image " + file + " :")
       if file == ".DS_Store":
           print("Removing dsStore file")
           os.remove("/Users/AtakanAtamert/Desktop/Screenshots/" + file)
       else:
           progressbar = tqdm(total=len(os.listdir("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/")))
       for i in os.listdir("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/"):
           if i == ".DS_Store":
               print("Removing dsStore file")
               os.remove("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/" + i)
               break
           arr.append("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/" + i)
           img = cv2.imread("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/" + i, 1)

           edges = cv2.Canny(img, 100, 200)
           indices = np.where(edges != [0])
           coordinates = list(zip(indices[0], indices[1]))
           # print(coordinates)

           yMax, dne = min(coordinates, key=itemgetter(0))
           yMin, dne1 = max(coordinates, key=itemgetter(0))
           dne2, xMax = min(coordinates, key=itemgetter(1))
           dne3, xMin = max(coordinates, key=itemgetter(1))

           f.write(str(xMax) + "   " + str(xMin) + "   " + str(yMax) + "   " + str(yMin) + "   " + file + "/" +i + "\n")
           # print(xMax, "   ", xMin, "   ", yMax, "   ", yMin, "   ", i)

           progressbar.update(1)

except IOError:
    print("Problem with file I/O!")
except KeyboardInterrupt:
    print("Operation Cancelled!")
except:
    print("An error occured!")

print("\n Coordinates successfully found!")
