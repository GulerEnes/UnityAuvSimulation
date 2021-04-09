import cv2 as cv2
import numpy as np
import os
from time import time
from window_capture import WindowCapture

# Change the working directory to the folder this script is in.
# Doing this because I'll be putting the files from each video in their own folder on GitHub
os.chdir(os.path.dirname(os.path.abspath(__file__)))


# initialize the WindowCapture class
wincap = WindowCapture('RowSimulator - SampleScene - PC, Mac & Linux Standalone - Unity 2019.4.17f1 Personal <DX11>')
loop_time = time()
def getContours(img,imgContour):
    contours, hierarchy = cv2.findContours(img, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
    for cnt in contours:
        M = cv2.moments(cnt)
        cntX = int(M["m10"] / M["m00"])
        cntY = int(M["m01"] / M["m00"])
        area = cv2.contourArea(cnt)

        if area > 5000 and area < 150000:
            cv2.drawContours(imgContour, cnt, -1, (255, 0, 255), 7)
            peri = cv2.arcLength(cnt, True)
            approx = cv2.approxPolyDP(cnt, 0.02 * peri, True)
            if len(approx) == 8:
                x, y, w, h = cv2.boundingRect(approx)
                cv2.rectangle(imgContour, (x, y), (x + w, y + h), (255, 0, 0), 5)

                cv2.putText(imgContour, "Points: " + str(len(approx)), (x + w + 20, y + 20), cv2.FONT_HERSHEY_COMPLEX,
                            .7,
                            (0, 255, 0), 2)
                cv2.putText(imgContour, "Area: " + str(int(area)), (x + w + 20, y + 45), cv2.FONT_HERSHEY_COMPLEX, 0.7,
                            (0, 255, 0), 2)
                cv2.circle(imgContour, (cntX, cntY), 7, (255, 255, 255), -1)
                cv2.circle(imgContour, (cntX, int((h) / 6) + y), 7, (0, 0, 0), -1)
                print("Center of the image is :",cntX," ",cntY )
                print("Width: " + str(w))
                print("Height: " + str(h))
                print("x:" + str(x))
                print("y:" + str(y))
while(True):

    # get an updated image of the game
    img = wincap.get_screenshot()

    imgContour = img.copy()
    imgBlur = cv2.GaussianBlur(img, (7, 7), 1)
    imgGray = cv2.cvtColor(imgBlur, cv2.COLOR_BGR2GRAY)

    imgCanny = cv2.Canny(imgGray, 23, 20)
    kernel = np.ones((5, 5))
    imgDil = cv2.dilate(imgCanny, kernel, iterations=1)
    getContours(imgDil, imgContour)




    cv2.imshow('Computer Vision', imgContour)

    # debug the loop rate
    print('FPS {}'.format(1 / (time() - loop_time)))
    loop_time = time()

    # press 'q' with the output window focused to exit.
    # waits 1 ms every loop to process key presses
    if cv2.waitKey(1) == ord('q'):
        cv2.destroyAllWindows()
        break

print('Done.')
