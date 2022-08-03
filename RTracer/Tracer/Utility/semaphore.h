#pragma once

struct semaphore
{
	int mutex = 0;
	int n_waiting = 0;
	semaphore(int m)
	{
		mutex = m;
	}
};

void wait(semaphore* s)
{
	s->n_waiting++;
	while (s->mutex == 0);
	s->mutex--;
	s->n_waiting--;
}

void signal(semaphore* s)
{
	s->mutex++;
}