#pragma once
#include <string>
#include <semaphore.h>

class ImageOutputSynch
{
	semaphore* mutex;
public:
	ImageOutputSynch(std::string file_name)
	{
		mutex = new semaphore(1);
	}

	void write_to_file()
	{
		wait(mutex);
	}
};