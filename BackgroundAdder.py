import cv2
import os
from tqdm import tqdm

arr = []
count = 0
progressbar = 0
for file in os.listdir("/Users/AtakanAtamert/Desktop/Screenshots/"):
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
		img = cv2.imread("/Users/AtakanAtamert/Desktop/Screenshots/" + file + "/" + i, -1)

		background = cv2.imread("street-wallpaper.jpg")

		resizedBackground = []
		x, y = 0, 0

		if img.shape[0] != background.shape[0] and img.shape[1] != background.shape[1]:
			resizedBackground = cv2.resize(background, (int(img.shape[1]), int(img.shape[0])))

		b, g, r, a = cv2.split(img)

		overlayColor = cv2.merge((b, g, r))

		mask = cv2.medianBlur(a, 5)

		h, w, _ = overlayColor.shape

		roi = resizedBackground[y: y + h, x: x + w]

		imgBg = cv2.bitwise_and(roi.copy(), roi.copy(), mask=cv2.bitwise_not(mask))
		imgFg = cv2.bitwise_and(overlayColor, overlayColor, mask=mask)

		concatenatedImg = cv2.add(imgBg, imgFg)

		newpath = "/Users/AtakanAtamert/Desktop/ScenedImages/"
		if not os.path.exists(newpath):
			os.makedirs(newpath)

		cv2.imwrite(newpath + str(count) + ".png", concatenatedImg)
		count += 1
		progressbar.update(1)




