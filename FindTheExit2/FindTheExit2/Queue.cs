using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoublyLinkedLists;

namespace FindTheExit2
{
    /// <summary>
    /// Represents a generic queue.
    /// </summary>
    /// <typeparam name="T">Generic type parament.</typeparam>
    class Queue<T>
    {
        /// <summary>
        /// Gets or sets the head node in the queue.
        /// </summary>
        public Node<T> Head
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the tail node in the queue.
        /// </summary>
        public Node<T> Tail
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the size of the queue.
        /// </summary>
        public int Size
        {
            get; set;
        }

        /// <summary>
        /// Initializes an instance of the queue with default head, tail and size.
        /// </summary>
        public Queue()
        {
            Head = null;
            Tail = null;
            Size = 0;
        }

        /// <summary>
        /// Returns true if the queue is empty, else returns false.
        /// </summary>
        /// <returns>True if the queue is empty, else false.</returns>
        public bool IsEmpty()
        {
            return Size == 0;
        }

        /// <summary>
        /// Creates a new node with a specified element and adds it to the tail of the queue.
        /// </summary>
        /// <param name="element">The specified element for the new node.</param>
        public void Enqueue(T element)
        {
            Node<T> node = new Node<T>(element);

            if (!IsEmpty())
            {
                Tail.Next = node;
                Tail = node;
            }
            else
            {
                Head = node;
                Tail = node;
            }

            Size++;
        }

        /// <summary>
        /// Throws an ApplicationException when the queue is empty.
        /// </summary>
        public void EmptyQueueThrowException()
        {
            if (IsEmpty())
            {
                throw new ApplicationException("The queue is empty.");
            }
        }

        /// <summary>
        /// Returns the head node's element in the queue without removing it.
        /// </summary>
        /// <returns>The head node's element in the queue.</returns>
        public T Front()
        {
            EmptyQueueThrowException();
     
            return Head.Element;
        }

        /// <summary>
        /// Returns the head node's element in the queue and removes it.
        /// </summary>
        /// <returns>The head node's element in the queue.</returns>
        public T Dequeue()
        {
            EmptyQueueThrowException();

            T headElement = Head.Element;

            if (Size == 1)
            {
                Head = null;
                Tail = null;
            }
            else
            {
                Head = Head.Next;
            }

            Size--;

            return headElement;
        }

        /// <summary>
        /// Empty all nodes (elements) from the list.
        /// </summary>
        public void Clear()
        {
            Head = null;
            Tail = null;
            Size = 0;
        }
    }
}
