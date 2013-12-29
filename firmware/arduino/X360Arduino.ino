#include <SPI.h>

enum X360Buttons
{
	XBOX_A     =     1,
	XBOX_B     =     2,
	XBOX_X     =     4,
	XBOX_Y     =     8,
	XBOX_LB    =    16,
	XBOX_RB    =    32,
	XBOX_Back  =    64,
	XBOX_Start =   128,
	XBOX_Guide =   256,
	XBOX_Sync  =   512,
	XBOX_Up    =  1024,
	XBOX_Down  =  2048,
	XBOX_Left  =  4096,
	XBOX_Right =  8192,
	XBOX_LS    = 16384,
	XBOX_RS    = 32768
};

class X360Controller
{
	public:
		X360Controller()
		{
			pinMode(this->slaveSelectPin, OUTPUT);

			for (int i = 2; i <= 9; i++)
			{
				pinMode (i, OUTPUT);
			}

			pinMode(A0, OUTPUT);
			pinMode(A1, OUTPUT);
			pinMode(A2, OUTPUT);
			pinMode(A3, OUTPUT);
			pinMode(A4, OUTPUT);
			pinMode(A5, OUTPUT);

			SPI.begin();

			resetController();
		};

		void executePacketCommands(const unsigned char* data)
		{
			int code = data[6] | (data[7] << 8) | (data[8] << 16)  | (data[9] << 24);
   
			int lsVertical = data[0];
			int lsHorizontal = data[1];
   
			int rsVertical = data[2];
			int rsHorizontal = data[3];
   
			int rt = data[4];
			int lt = data[5];
   
			moveLeftStick(lsHorizontal, lsVertical);
			moveRightStick(rsHorizontal, rsVertical);
			pressRightTrigger(rt);
			pressLeftTrigger(lt);
			pressButtons(code);
		};
  
	private:
		void moveLeftStick(int x, int y)
		{
			digitalPotWrite(this->lsHorizontalChannell, x);
			digitalPotWrite(this->lsVerticalChannell, y);
		};

		void moveRightStick(int x, int y)
		{
			digitalPotWrite(this->rsHorizontalChannell, x);
			digitalPotWrite(this->rsVerticalChannell, y);
		};

		void pressLeftTrigger(int value)
		{
			digitalPotWrite(this->leftTriggerChannell, value);
		};

		void pressRightTrigger(int value)
		{
			digitalPotWrite(this->rightTriggerChannell, value);
		};

		void pressButtons(int buttonStates)
		{
			pressButton(buttonStates, XBOX_Guide, this->guidePin);
			pressButton(buttonStates, XBOX_Start, this->startPin);
			pressButton(buttonStates, XBOX_Back, this->backPin);

			pressButton(buttonStates, XBOX_A, this->aPin);
			pressButton(buttonStates, XBOX_B, this->bPin);
			pressButton(buttonStates, XBOX_Y, this->yPin);
			pressButton(buttonStates, XBOX_X, this->xPin);

			pressButton(buttonStates, XBOX_RS, this->rsPin);
			pressButton(buttonStates, XBOX_LS, this->lsPin);

			pressButton(buttonStates, XBOX_RB, this->rbPin);
			pressButton(buttonStates, XBOX_LB, this->lbPin);

			pressButton(buttonStates, XBOX_Up, this->upPin);
			pressButton(buttonStates, XBOX_Right, this->rightPin);
			pressButton(buttonStates, XBOX_Down, this->downPin);
			pressButton(buttonStates, XBOX_Left, this->leftPin);
		};
    
		void resetController()
		{
			moveLeftStick(127, 127);
			moveRightStick(127, 127);
			pressRightTrigger(0);
			pressLeftTrigger(0);
			pressButtons(0);
		};

		void pressButton(int state, int key, int pin)
		{
			if ((state & key) == key)
			{
				digitalWrite(pin, HIGH);
			}
			else
			{
				digitalWrite(pin, LOW);
			}      
		};

		void digitalPotWrite(int address, int value)
		{
			digitalWrite(slaveSelectPin, LOW);

			SPI.transfer(address);
			SPI.transfer(value);

			digitalWrite(slaveSelectPin, HIGH);
		};

	private:
		static const int lsVerticalChannell = 4;
		static const int lsHorizontalChannell = 5;
		static const int rsVerticalChannell = 1;
		static const int rsHorizontalChannell = 3;
		static const int leftTriggerChannell = 2;
		static const int rightTriggerChannell = 0;
		static const int guidePin = 7;
		static const int aPin = 9;
		static const int bPin = 6;
		static const int yPin = 5;
		static const int xPin = 4;
		static const int rsPin = 2;
		static const int lsPin = 3;
		static const int rbPin = A2;
		static const int lbPin = A3;
		static const int startPin = A4;
		static const int backPin = A5;
		static const int upPin = A0;
		static const int rightPin = 8;
		static const int downPin = A1;
		static const int leftPin = 12;
		static const int slaveSelectPin = 10;
};

X360Controller* x360 = NULL;

void setup()
{
	Serial.begin(115200);

	x360 = new X360Controller();
};

#define PACKET_SIZE			11
#define MAX_MILLIS_TO_WAIT	1000
unsigned long starttime;

void loop()
{
	starttime = millis();

	unsigned char data[PACKET_SIZE] = { 0 };

	while ((Serial.available() < PACKET_SIZE) && ((millis() - starttime) < MAX_MILLIS_TO_WAIT))
	{
		// hang in this loop until we either get 11 bytes of data or 1 second has gone by
		// Serial.println("Waiting...");
	}

	if (Serial.available() < PACKET_SIZE)
	{
		// the data didn't come in - handle that problem here
		// Serial.println("ERROR - Didn't get 11 bytes of data!");
	}
	else
	{
		for (int n = 0; n < PACKET_SIZE; n++)
		{
			data[n] = Serial.read(); // Then: Get them.
		}

		unsigned char crc = data[10];

		if (crc != 0xff)
		{
			Serial.print("Invalid data received: ");
			Serial.println(crc, DEC);   
      
			while ((Serial.available() > 0))
			{
				int endCode = Serial.read();
        
				if (endCode == 0xff)
				{
					Serial.println("Got end code");
					return;
				}
				else
				{
					Serial.print("Waiting for end code: ");
					Serial.println(endCode, DEC);   
				}
			}
		}

		x360->executePacketCommands(data);
	}
};
