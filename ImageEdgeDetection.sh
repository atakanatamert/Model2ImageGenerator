echo "----------------------------------> Starting to process images"
cd ./Desktop/Screenshots
ls -ld .?* 
rm .DS_Store
cd ~/Desktop
python3 ImageEdgeDetection.py
python3 BackgroundAdder.py
echo "----------------------------------> Image processing finished !!!!"
 