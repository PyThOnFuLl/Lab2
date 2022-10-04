import gspread
import numpy as np

# define data, and change list to array
x = [3, 21, 22, 34, 54, 34, 55, 67, 89, 99]
x = np.array(x)
y = [2, 22, 24, 65, 79, 82, 55, 130, 150, 199]
y = np.array(y)


# The basic linear regression model is wx+ b, and since this is a two-dimensional space, the model is ax+ b
def model(a, b, x):
    return a * x + b


# Tahe most commonly used loss function of linear regression model is the loss function of mean variance difference
def loss_function(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)

    return (0.5 / num) * (np.square(prediction - y)).sum()


# The optimization function mainly USES partial derivatives to update two parameters a and b
def optimize(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)

    # Update the values of A and B by finding the partial derivatives of the loss function on a and b
    da = (1.0 / num) * ((prediction - y) * x).sum()
    db = (1.0 / num) * ((prediction - y).sum())
    a = a - Lr * da
    b = b - Lr * db

    return a, b


# iterated function, return a and b
def iterate(a, b, x, y, times):
    for i in range(times):
        a, b = optimize(a, b, x, y)
    return a, b


# Initialize parameters and display
a = np.random.rand(1)
b = np.random.rand(1)

Lr = 0.000001

gc = gspread.service_account(filename='gamedev-364414-8647bd6528a6.json')
sh = gc.open("RegressionSheets")

old_loss = 0

for i in range(10):
    a, b = iterate(a, b, x, y, 100 * (i + 1))

    prediction = model(a, b, x)
    loss = loss_function(a, b, x, y)

    diff_loss = abs(loss - old_loss)
    old_loss = loss

    sh.sheet1.update(('A' + str(i + 1)), str(i + 1))
    sh.sheet1.update(('B' + str(i + 1)), str(loss))
    sh.sheet1.update(('C' + str(i + 1)), str(diff_loss))