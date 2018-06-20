
#define LIGHT_SENSOR_PIN 2
#define SOUND_SENSOR_PIN 3

#define LIGHT_FILL_TIME_MILISECONDS 500.0
#define SOUND_FILL_TIME_MILISECONDS 10.0
#define SAMPLE_RATE_MILISECONDS 1
#define BIT_RANGE 8
#define LED_B 7

//unsigned short lightCounter = 0;
//unsigned short soundCounter = 0;

float lightCounter = 0.0;
float soundCounter = 0.0;

float lightAdd = 0.0;
float soundAdd = 0.0;

unsigned short maxDec = 0;

String serialSend;

void setup()
{

	pinMode(LIGHT_SENSOR_PIN, INPUT);
	pinMode(SOUND_SENSOR_PIN, INPUT);
    pinMode(LED_B, INPUT);

	Serial.begin(9600);
	delay(1000);

	maxDec = pow(2, BIT_RANGE) - 1;
	lightCounter = maxDec;
	soundCounter = maxDec;

	lightAdd = (maxDec / LIGHT_FILL_TIME_MILISECONDS) * SAMPLE_RATE_MILISECONDS;
	soundAdd = (maxDec / SOUND_FILL_TIME_MILISECONDS) * SAMPLE_RATE_MILISECONDS;
}

// OFFICIAL LOOP
void loop()
{
	
	if (digitalRead(LIGHT_SENSOR_PIN) == LOW){
        if (lightCounter < 255)
            if ((lightCounter + lightAdd) <= 255)
                lightCounter += lightAdd;
            else
                lightCounter = 255;
	}
	else {
		if (lightCounter > 0)
			if ((lightCounter - lightAdd) >= 0)
				lightCounter -= lightAdd;
			else
				lightCounter = 0;
	}

    if (digitalRead(SOUND_SENSOR_PIN) == HIGH)
    {
        digitalWrite (LED_B, HIGH);
    }
    else
    {
        digitalWrite (LED_B, LOW);
    }
    
	if (digitalRead(SOUND_SENSOR_PIN) == HIGH){
		if (soundCounter < 255)
			if ((soundCounter + soundAdd) <= 255)
				soundCounter += soundAdd;
			else
				soundCounter = 255;
	}
	else {
		if (soundCounter > 0)
			if ((soundCounter - soundAdd) >= 0)
				soundCounter -= soundAdd;
			else
				soundCounter = 0;
	}

	if (Serial.available()) {

		int c = Serial.read();
    	if (c == '?'){
            // for (int i = 0; i < 10; i++){
                // digitalWrite(LED_B,LOW);
                // delay(200);
                // digitalWrite(LED_B,HIGH);
                // delay(200);
            // }
            
            String lightString = "000";
            if ((int) lightCounter > 100)
                lightString = String ((int) lightCounter);
            else if ((int) lightCounter > 10)
                lightString = "0" + String ((int) lightCounter);
            else
                lightString = "00" + String ((int) lightCounter);
                
            String soundString = "000";
            if ((int) soundCounter > 100)
                soundString = String ((int) soundCounter);
            else if ((int) soundCounter > 10)
                soundString = "0" + String ((int) soundCounter);
            else
                soundString = "00" + String ((int) soundCounter);
            
            
    		//serialSend = String((int) lightCounter) + "_" + String((int) soundCounter) + "#";
            serialSend = lightString + "_" + soundString + "#";
			Serial.print(serialSend);
            Serial.flush ();
			//Serial.flush();
			//delay(10);
			//delay(SAMPLE_RATE_MILISECONDS);
    	}
	}
	delay(SAMPLE_RATE_MILISECONDS);
}

