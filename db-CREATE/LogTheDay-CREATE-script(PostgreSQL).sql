CREATE DATABASE "LogTheDay" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
\connect "LogTheDay"

CREATE TABLE public.users (
    id uuid NOT NULL,
    name character varying(40) NOT NULL,
    email character varying NOT NULL UNIQUE,
    password_hash character varying NOT NULL,
    avatar_img character varying,
    reg_date date DEFAULT now() NOT NULL,
    reg_time time without time zone DEFAULT now() NOT NULL,
    last_login_date date DEFAULT now() NOT NULL,
    CONSTRAINT user_id_pk PRIMARY KEY (id)
);

CREATE TABLE public.pages (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    title character varying DEFAULT 'Page'::character varying,
    description character varying DEFAULT 'Description...'::character varying,
    icon_link character varying,
    type character varying DEFAULT 'plain'::character varying NOT NULL,
    privacy_type integer DEFAULT 0 NOT NULL,
    custom_css character varying,
    creation_date date DEFAULT now() NOT NULL,
    creation_time timestamp without time zone DEFAULT now() NOT NULL,
    last_modified_date date DEFAULT now() NOT NULL,
    last_modified_time timestamp without time zone DEFAULT now() NOT NULL,
    CONSTRAINT pages_id_pk PRIMARY KEY (id),
    CONSTRAINT user_id_fk FOREIGN KEY (user_id) REFERENCES public.users(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE public.notes (
    id uuid NOT NULL,
    page_id uuid NOT NULL,
    title character varying,
    description character varying,
    status character varying,
    priority integer DEFAULT 5,
    score integer DEFAULT 1,
    icon_link character varying,
    creation_date date NOT NULL,
    creation_time time without time zone NOT NULL,  
    last_modified_date date DEFAULT now() NOT NULL,
    last_modified_time timestamp without time zone DEFAULT now() NOT NULL,
    primary_color character varying,    
    secondary_color character varying,
    CONSTRAINT note_id_pk PRIMARY KEY (id),
    CONSTRAINT page_id_fk FOREIGN KEY (page_id) REFERENCES public.pages(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE public.tags (
    id uuid NOT NULL,
    page_id uuid NOT NULL,
    title character varying NOT NULL UNIQUE,
    description character varying,
    CONSTRAINT tag_id_pk PRIMARY KEY (id),
    CONSTRAINT unique_tag UNIQUE (page_id, title)
);

CREATE TABLE public.notes_tags (
    note_id uuid NOT NULL,
    tag_id uuid NOT NULL,
    CONSTRAINT note_tag_pk PRIMARY KEY (note_id, tag_id),
    CONSTRAINT note_id_fk FOREIGN KEY (note_id) REFERENCES public.notes(id) ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT tag_id_fk FOREIGN KEY (tag_id) REFERENCES public.tags(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE public.attachments (
    id uuid NOT NULL,
    name character varying,
    type character varying DEFAULT 'file'::character varying NOT NULL,
    content_link character varying NOT NULL,
    file_type character varying,
    note_id uuid NOT NULL,
    CONSTRAINT attachments_id_pk PRIMARY KEY (id),
    CONSTRAINT note_id_fk FOREIGN KEY (note_id) REFERENCES public.notes(id) ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE INDEX fki_note_id ON public.attachments USING btree (note_id);
CREATE INDEX fki_user_content_pages_id ON public.pages USING btree (id);
