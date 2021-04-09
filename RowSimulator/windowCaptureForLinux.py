import time

import cv2
import mss
import numpy as np
import socket
import datetime
from time import sleep
from math import atan

HOST = "192.168.1.45"
PORT = 9600

def sendOutput(x, angle):
	print("called at ", datetime.datetime.now(), end=" -> ")
	print("x:", int(x), "    angle:", round(angle, 2))
	s = socket.socket()
	s.connect((HOST, PORT))
	s.send((str(int(x)) + ',' + str(round(angle, 2))).encode())
	s.close()

def getContours(img,imgContour):
	contours, hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
	for cnt in contours:
		area = cv2.contourArea(cnt)
		M = cv2.moments(cnt)
		cntX = int(M["m10"] / M["m00"])
		cntY = int(M["m01"] / M["m00"])
		if area > 0:
			cv2.drawContours(imgContour, cnt, -1, (255, 0, 255), 7)
			peri = cv2.arcLength(cnt, True)
			approx = cv2.approxPolyDP(cnt, 0.02 * peri, True)
			
			if len(approx) >= 7:
				x , y , w, h = cv2.boundingRect(approx)
				sendOutput(x+w/2, atan(h/w))

with mss.mss() as sct:
	# Part of the screen to capture
	monitor = {"top": 370, "left": 0, "width": 900, "height": 500}

	while "Screen capturing":
		last_time = time.time()

		# Get raw pixels from the screen, save it to a Numpy array
		img = np.array(sct.grab(monitor))
		img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
		imgContour = img.copy()
		blur = cv2.GaussianBlur(img,(7,7),1)

		gaus = cv2.adaptiveThreshold(blur, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY, 17, 2)
		ret2,th2 = cv2.threshold(img,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
		imgCanny = cv2.Canny(th2,100,200)
		kernel = np.ones((5, 5))
		imgDil = cv2.dilate(imgCanny, kernel, iterations=1)
		getContours(imgDil,imgContour)
		print("fps: {}".format(1 / (time.time() - last_time)))

