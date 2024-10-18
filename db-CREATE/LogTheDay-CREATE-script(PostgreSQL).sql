-- PostgreSQL database dump

-- Dumped from database version 17rc1
-- Dumped by pg_dump version 17rc1

CREATE DATABASE "LogTheDay" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
\connect "LogTheDay"

CREATE TABLE public.users (
    id uuid NOT NULL,
    name character varying(25) NOT NULL,
    email character varying NOT NULL,
    password_hash character varying NOT NULL,
    reg_date date NOT NULL,
    CONSTRAINT user_id_pk PRIMARY KEY (id)
);

CREATE TABLE public.pages (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    page_type character varying DEFAULT 'plain'::character varying NOT NULL,
    privacy_type integer DEFAULT 0 NOT NULL,
    custom_css character varying,
    CONSTRAINT pages_id_pk PRIMARY KEY (id),
    CONSTRAINT user_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE public.notes (
    id uuid NOT NULL,
    page_id uuid NOT NULL,
    creation_date date NOT NULL,
    creation_time time without time zone NOT NULL,
    title character varying,
    description character varying,
    tags character varying[],
    priority integer DEFAULT 5,
    bg_color character varying(20) DEFAULT 'rgb(28 28 28)'::character varying,
    text_color character varying(20) DEFAULT 'rgb(255 255 255)'::character varying,
    CONSTRAINT note_id_pk PRIMARY KEY (id),
    CONSTRAINT page_id_fk FOREIGN KEY (page_id) REFERENCES public.pages(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE public.attachments (
    id uuid NOT NULL,
    name character varying,
    type character varying DEFAULT 'file'::character varying NOT NULL,
    file_type character varying,
    note_id uuid NOT NULL,
    CONSTRAINT attachments_id_pk PRIMARY KEY (id),
    CONSTRAINT note_id_fk FOREIGN KEY (note_id) REFERENCES public.notes(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE INDEX fki_note_id ON public.attachments USING btree (note_id);
CREATE INDEX fki_user_content_pages_id ON public.pages USING btree (id);
